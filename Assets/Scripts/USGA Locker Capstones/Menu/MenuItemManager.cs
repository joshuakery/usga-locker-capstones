using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using JoshKery.GenericUI.Events;
using JoshKery.GenericUI.DOTweenHelpers;
using DG.Tweening;

namespace JoshKery.USGA.LockerCapstones
{
    public class MenuItemManager : LockerCapstonesWindow
    {
        
        private static ListIntEvent _onCategorySelectedCallback;

        /// <summary>
        /// Callback event to be invoked after SetSelectedCategory(int contentTrailID)
        /// </summary>
        public static ListIntEvent onCategorySelectedCallback
        {
            get
            {
                if (_onCategorySelectedCallback == null)
                    _onCategorySelectedCallback = new ListIntEvent();

                return _onCategorySelectedCallback;
            }
        }

        private static UnityEvent _onFiltered;

        /// <summary>
        /// Callback event to be invoked after Filter()
        /// </summary>
        public static UnityEvent onFiltered
        {
            get
            {
                if (_onFiltered == null)
                    _onFiltered = new UnityEvent();

                return _onFiltered;
            }
        }

        private MenuItem[] childMenuItems;

        public List<int> selectedContentTrailIDs;

        private MenuItem justClickedItem;

        #region Monobehaviour Methods

        protected override void OnEnable()
        {
            base.OnEnable();

            FilterDrawer.onFilterClicked += SetSelectedCategoryAndFilter;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            FilterDrawer.onFilterClicked -= SetSelectedCategoryAndFilter;
        }
        #endregion

        protected override void ResetChildWindows()
        {
            base.ResetChildWindows();

            childMenuItems = GetComponentsInChildren<MenuItem>();
        }

        public override void SetContent()
        {
            if (appState == null) { return; }

            ClearAllDisplays();

            List<LockerProfile> lockerProfiles = appState.data?.lockerProfiles;
            if (lockerProfiles != null)
            {
                foreach (LockerProfile lockerProfile in lockerProfiles)
                {
                    MenuItem display = InstantiateDisplay<MenuItem>();
                    display.SetContent(lockerProfile);

                    int id = lockerProfile.id;
                    display.gameObject.name = string.Format("Menu Button: {0} {1} {2}", id, lockerProfile.firstName, lockerProfile.lastName);
                    display.button.onClick.AddListener(() => { MainCanvasStateMachine.onAnimateToProfile.Invoke(id); });
                    display.button.onClick.AddListener(() => { OnItemClick(display); });
                }
            }

            ResetChildWindows();
        }

        #region Filter Methods
        protected override void MidPulseCallback()
        {
            Filter();
        }

        public void Filter()
        {
            if (childMenuItems != null && childMenuItems.Length > 0)
            {
                if (selectedContentTrailIDs != null && selectedContentTrailIDs.Count > 0)
                {
                    foreach (MenuItem menuItem in childMenuItems)
                    {
                        bool hasAtLeastOneCategoryInCommon = selectedContentTrailIDs.Intersect(menuItem.contentTrailIDs).Any();
                        menuItem.gameObject.SetActive(hasAtLeastOneCategoryInCommon);
                    }
                }
                else
                {
                    foreach (MenuItem menuItem in childMenuItems)
                    {
                        menuItem.gameObject.SetActive(true);
                    }
                }

                onFiltered.Invoke();
            }
        }

        private void SetSelectedCategory(int contentTrailID)
        {
            if (selectedContentTrailIDs == null)
                selectedContentTrailIDs = new List<int>();

            selectedContentTrailIDs.Clear();

            selectedContentTrailIDs.Add(contentTrailID);

            onCategorySelectedCallback.Invoke(selectedContentTrailIDs);
        }

        private void SetSelectedCategoryAndFilter(int contentTrailID)
        {
            SetSelectedCategory(contentTrailID);
            Pulse(SequenceType.Join);
        }
        #endregion

        #region Menu Out and In Animation Methods
        /// <summary>
        /// Subscribes to MenuItem's button's onClick event so that we can know which item was clicked to do a dynamic animation
        /// </summary>
        /// <param name="item"></param>
        private void OnItemClick(MenuItem item)
        {
            justClickedItem = item;
        }
        public void CloseAllItemsButSelected()
        {
            foreach (MenuItem item in childMenuItems)
            {
                if (item != justClickedItem)
                    item.Close(SequenceType.UnSequenced);
            }
        }

        public void SpecialHighlightSelectedItem()
        {
            if (justClickedItem != null)
                justClickedItem._SpecialHighlight();
        }

        public void SpecialCloseSelectedItem()
        {
            if (justClickedItem != null)
                justClickedItem._SpecialClose();
        }

        protected override Sequence _Close(SequenceType sequenceType = SequenceType.UnSequenced, float atPosition = 0)
        {
            Sequence wrapper = DOTween.Sequence();

            wrapper.Join(base._Close(SequenceType.UnSequenced, atPosition));

            //Close All Items
            foreach (MenuItem item in childMenuItems)
            {
                Tween tween = item.Close(SequenceType.UnSequenced);
                if (tween != null)
                    wrapper.Join(tween);
            }

            sequenceManager.CreateSequenceIfNull();
            AttachTweenToSequence(sequenceType, wrapper, sequenceManager.currentSequence, false, 0, null);

            return wrapper;
        }

        public void CloseImmediately()
        {
            _Close(SequenceType.CompleteImmediately);
        }

        protected override Sequence _Open(SequenceType sequenceType = SequenceType.UnSequenced, float atPosition = 0)
        {
            Sequence wrapper = DOTween.Sequence();

            wrapper.Join(base._Open(SequenceType.UnSequenced, atPosition));

            //Open All Items with offset in their order
            for (int i=0; i<childMenuItems.Length; i++)
            {
                MenuItem item = childMenuItems[i];

                Tween tween = item.Open(SequenceType.UnSequenced);
                if (tween != null)
                    wrapper.Insert(i * 0.05f, tween);
            }

            sequenceManager.CreateSequenceIfNull();
            AttachTweenToSequence(sequenceType, wrapper, sequenceManager.currentSequence, false, 0, null);

            return wrapper;
        }
        #endregion
    }

}


