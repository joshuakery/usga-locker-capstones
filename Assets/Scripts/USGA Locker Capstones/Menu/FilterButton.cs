using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.DOTweenHelpers.FlexibleUI;
using UnityEngine.EventSystems;

namespace JoshKery.USGA.LockerCapstones
{
    [RequireComponent(typeof(Button))]
    public class FilterButton : ColorChangeButton
    {
        private Button button;

        [SerializeField]
        private FilterButtonsCabinet filterButtonsCabinet;

        [SerializeField]
        private MenuItemManager menuItemManager;

        private bool isAFilterActive
        {
            get
            {
                if (menuItemManager != null)
                {
                    if (menuItemManager.selectedContentTrailIDs != null &&
                        !menuItemManager.selectedContentTrailIDs.Contains(-1)
                        )
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        private bool isFilterButtonsCabinetOpen
        {
            get
            {
                if (filterButtonsCabinet != null)
                    return filterButtonsCabinet.isOpen;
                else
                    return false;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            button = GetComponent<Button>();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            InvertGraphics();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            //do nothing
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            InvertGraphics();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            SetGraphics();
        }

        private void OnEnable()
        {
            if (button != null)
                button.onClick.AddListener(OnClick);

            MainCanvasStateMachine.afterAnimateToMenu += AfterAnimateToMenu;
        }

        private void OnDisable()
        {
            if (button != null)
                button.onClick.RemoveListener(OnClick);

            MainCanvasStateMachine.afterAnimateToMenu -= AfterAnimateToMenu;
        }

        private void AfterAnimateToMenu()
        {
            SetGraphics();
        }

        private void OnClick()
        {
            if (isAFilterActive)
                FilterDrawer.onFilterClicked(-1);

            if (filterButtonsCabinet != null)
                filterButtonsCabinet.DoToggle();

            StartCoroutine(SetGraphicsCo());
        }

        private IEnumerator SetGraphicsCo()
        {
            yield return null;
            SetGraphics();
        }

        private void SetGraphics()
        {
            if (buttonWindow == null) return;

            buttonWindow.sequenceManager.CompleteCurrentSequence();

            if (isAFilterActive || isFilterButtonsCabinetOpen)
                buttonWindow.Close(GenericUI.DOTweenHelpers.SequenceType.CompleteImmediately);
            else
                buttonWindow.Open(GenericUI.DOTweenHelpers.SequenceType.CompleteImmediately);
        }

        private void InvertGraphics()
        {
            if (buttonWindow == null) return;

            buttonWindow.sequenceManager.CompleteCurrentSequence();

            if (isAFilterActive || isFilterButtonsCabinetOpen)
                buttonWindow.Open(GenericUI.DOTweenHelpers.SequenceType.CompleteImmediately);
            else
                buttonWindow.Close(GenericUI.DOTweenHelpers.SequenceType.CompleteImmediately);
        }
    }
}


