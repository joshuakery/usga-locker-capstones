using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

        private bool isSelected;

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


        public Button button;

        public bool isInEra = false;
        
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

            MainCanvasStateMachine.onAnimateToMenu.AddListener(OnAnimateToMenu);
            MainCanvasStateMachine.onAnimateToProfile.AddListener(OnAnimateToProfile);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            MenuItemManager.onCategorySelectedCallback.RemoveListener(OnCategorySelected);

            MainCanvasStateMachine.onAnimateToMenu.RemoveListener(OnAnimateToMenu);
            MainCanvasStateMachine.onAnimateToProfile.RemoveListener(OnAnimateToProfile);
        }
        #endregion

        private void OnAnimateToMenu()
        {
            button.interactable = isInEra;
        }

        private void OnAnimateToProfile(int id)
        {
            button.interactable = isInEra;
        }

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
        /// <returns>Tween of the animation</returns>
        public Tween SetInteractable()
        {
            if (isInEra)
            {
                button.interactable = true;
                return _WindowAction(interactableSequence, SequenceType.CompleteImmediately);
            }
                
            else
            {
                button.interactable = false;
                return _WindowAction(disabledSequence, SequenceType.CompleteImmediately);
            }
                
        }
        #endregion

        #region Private Methods > Subscribers
        /// <summary>
        /// Subscribes to menuItemManager.onCategorySelectedCallback
        /// </summary>
        /// <param name="selectedContentTrailIDs"></param>
        private void OnCategorySelected(List<int> selectedContentTrailIDs)
        {
            isSelected = selectedContentTrailIDs.Contains(contentTrailID);
            SetAsSelected();
        }

        /// <summary>
        /// Sets the UI to highlight the button as selected or not
        /// </summary>
        /// <param name="isSelected">Is this button selected?</param>
        private void SetAsSelected()
        {
            if (button == null) return;

            if (isInEra)
            {
                if (isSelected)
                    _WindowAction(selectSequence, SequenceType.CompleteImmediately);
                else
                    _WindowAction(deselectSequence, SequenceType.CompleteImmediately);
            }
            else
            {
                SetInteractable();
            }
        }
        #endregion

    }
}


