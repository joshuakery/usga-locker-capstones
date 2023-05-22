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

            FilterOptionButton.onFilterClicked += SetSelectedCategoryAndFilter;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            FilterOptionButton.onFilterClicked -= SetSelectedCategoryAndFilter;
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

        public void Filter()
        {
            if (childMenuItems != null && childMenuItems.Length > 0)
            {
                if (selectedCategories != null && selectedCategories.Count > 0)
                {
                    foreach (MenuItem menuItem in childMenuItems)
                    {
                        bool hasAtLeastOneCategoryInCommon = selectedCategories.Intersect(menuItem.categories).Any();
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
                
            }
        }

        private void SetSelectedCategory(string category)
        {
            if (selectedCategories == null)
                selectedCategories = new List<string>();

            selectedCategories.Clear();

            if (!string.IsNullOrEmpty(category))
                selectedCategories.Add(category);
        }

        private void SetSelectedCategoryAndFilter(string category)
        {
            SetSelectedCategory(category);
            Pulse(SequenceType.Join);
        }
    }

}


