using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class MenuItemManager : BaseWindow
    {
        private MenuItem[] childMenuItems;

        [SerializeField]
        private UIAnimationSequenceData pulseSequence;

        public List<string> selectedCategories;

        protected override void Awake()
        {
            base.Awake();

            childMenuItems = GetComponentsInChildren<MenuItem>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            FilterOptionButton.onFilterClicked += SetSelectedCategory;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            FilterOptionButton.onFilterClicked -= SetSelectedCategory;
        }

        //-------------------------------------Pulse--------------------------------------------
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

        //-------------------------------------Filter--------------------------------------------

        private void Filter()
        {
            if (childMenuItems != null && childMenuItems.Length > 0)
            {
                foreach (MenuItem menuItem in childMenuItems)
                {
                    bool hasAtLeastOneCategoryInCommon = selectedCategories.Intersect(menuItem.categories).Any();
                    menuItem.gameObject.SetActive(hasAtLeastOneCategoryInCommon);
                }
            }
        }

        public void SetSelectedCategory(string category)
        {
            if (selectedCategories == null)
                selectedCategories = new List<string>();

            selectedCategories.Clear();

            selectedCategories.Add(category);
        }


    }

}


