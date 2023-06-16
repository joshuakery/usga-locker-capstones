using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.Accordion;

namespace JoshKery.USGA.LockerCapstones
{
    public class FilterOptionButton : AccordionElement
    {
        public delegate void FilterClickedDelegate(int contentTrailID);
        public static FilterClickedDelegate onFilterClicked;

        /// <summary>
        /// ID value for filtering
        /// Is sent via onFilterClicked delegate to MenuItemManager
        /// </summary>
        public int contentTrailID;

        /// <summary>
        /// UI display
        /// </summary>
        [SerializeField]
        private TMP_Text nameField;

        /// <summary>
        /// Animation sequence to play when button is selected
        /// </summary>
        [SerializeField]
        private UIAnimationSequenceData selectSequence;

        /// <summary>
        /// Animation sequence to play when button is deselected
        /// </summary>
        [SerializeField]
        private UIAnimationSequenceData deselectSequence;

        private Button _button;
        public Button button
        {
            get
            {
                if (_button == null)
                    _button = GetComponent<Button>();

                return _button;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            MenuItemManager.onCategorySelectedCallback.AddListener(OnCategorySelected);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            MenuItemManager.onCategorySelectedCallback.RemoveListener(OnCategorySelected);
        }

        /// <summary>
        /// Listener for Button's onClick UnityEvent, calls static onFilterClicked delegate
        /// </summary>
        public void OnFilterClicked()
        {
            onFilterClicked(contentTrailID);
        }

        public void SetContent(ContentTrail contentTrail)
        {
            if (contentTrail != null)
            {
                contentTrailID = contentTrail.id;

                if (nameField != null)
                {
                    nameField.text = contentTrail.name;
                }
            }
        }

        /// <summary>
        /// Subscribes to menuItemManager.onCategorySelectedCallback
        /// </summary>
        /// <param name="selectedContentTrailIDs"></param>
        private void OnCategorySelected(List<int> selectedContentTrailIDs)
        {
            SetAsSelected(selectedContentTrailIDs.Contains(contentTrailID));
        }

        /// <summary>
        /// Sets the UI to highlight the button as selected or not
        /// </summary>
        /// <param name="isSelected">Is this button selected?</param>
        private void SetAsSelected(bool isSelected)
        {
            if (isSelected)
                _WindowAction(selectSequence, SequenceType.UnSequenced);
            else
                _WindowAction(deselectSequence, SequenceType.UnSequenced);
        }
    }

}

