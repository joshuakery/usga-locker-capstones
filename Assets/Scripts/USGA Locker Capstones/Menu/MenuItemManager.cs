using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using JoshKery.GenericUI.Events;
using JoshKery.GenericUI.DOTweenHelpers;

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

        [SerializeField]
        private UIAnimationSequenceData pulseSequence;

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

        #region Pulse Animation Methods
        protected virtual void _Pulse(
            SequenceType sequenceType = SequenceType.UnSequenced,
            float atPosition = 0f
        )
        {
            _WindowAction(pulseSequence, sequenceType, atPosition);
        }

        public virtual void Pulse(SequenceType sequenceType)
        {
            _Pulse(sequenceType);
        }

        public virtual void Pulse(float atPosition)
        {
            _Pulse(SequenceType.Insert, atPosition);
        }
        #endregion

        #region Filter Methods

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

        public void CloseAllItems()
        {
            foreach (MenuItem item in childMenuItems)
            {
                item.Close(SequenceType.UnSequenced);
            }
        }

        

        public void CloseAllItemsImmediately()
        {
            foreach (MenuItem item in childMenuItems)
            {
                item.Close(SequenceType.CompleteImmediately);
            }
        }

        public void OpenAllItems()
        {
            foreach (MenuItem item in childMenuItems)
            {
                item.Open(SequenceType.UnSequenced);
            }
        }
        #endregion
    }

}


