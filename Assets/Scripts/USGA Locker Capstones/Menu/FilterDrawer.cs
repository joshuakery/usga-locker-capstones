using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    /// <summary>
    /// Custom BaseWindow for Filter Option Buttons.
    /// Other classes should listen for static delegate onFilterClicked
    /// which passes int property contentTrailID.
    /// Also contains custom UIAnimation Sequences for de/selected
    /// and disabled/interactable states.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class FilterDrawer : BaseWindow
    {
        #region Static Delegate
        /// <summary>
        /// Static delegate setup for menu to subscribe to
        /// </summary>
        /// <param name="contentTrailID"></param>
        public delegate void FilterClickedDelegate(int contentTrailID);
        public static FilterClickedDelegate onFilterClicked;
        #endregion

        #region FIELDS
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

        #region Custom UIAnimation Sequences
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

        /// <summary>
        /// Animation sequence to play when button is interactable
        /// </summary>
        [SerializeField]
        private UIAnimationSequenceData interactableSequence;

        /// <summary>
        /// Animation sequence to play when button is disabled
        /// </summary>
        [SerializeField]
        private UIAnimationSequenceData disabledSequence;
        #endregion


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
        #endregion

        #region Static Methods
        public static void ClearFilters()
        {
            onFilterClicked.Invoke(-1);
        }
        #endregion

        #region Monobehaviour Methods
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
        #endregion

        #region Public Methods
        /// <summary>
        /// Listener for Button's onClick UnityEvent, calls static onFilterClicked delegate
        /// </summary>
        public void OnFilterClicked()
        {
            if (onFilterClicked != null)
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
        /// Sets the UI to animate the button based on if it's interactable or not
        /// </summary>
        /// <param name="interactable"></param>
        /// <returns>Tween of the animation</returns>
        public Tween SetInteractable(bool interactable)
        {
            if (interactable)
                return _WindowAction(interactableSequence, SequenceType.UnSequenced);
            else
                return _WindowAction(disabledSequence, SequenceType.UnSequenced);
        }
        #endregion

        #region Private Methods > Subscribers
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
            if (button.interactable)
            {
                if (isSelected)
                    _WindowAction(selectSequence, SequenceType.UnSequenced);
                else
                    _WindowAction(deselectSequence, SequenceType.UnSequenced);
            }
            else
            {
                SetInteractable(false);
            }
        }
        #endregion


    }
}


