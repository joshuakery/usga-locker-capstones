using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.DOTweenHelpers.FlexibleUI;
using UnityEngine.EventSystems;

namespace JoshKery.USGA.LockerCapstones
{
    [RequireComponent(typeof(Button))]
    public class ReturnToEraOverviewButton : ColorChangeButton
    {
        private Button button;

        protected override void Awake()
        {
            base.Awake();

            button = GetComponent<Button>();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (buttonWindow != null)
                buttonWindow.Close();
        }

        private void OnEnable()
        {
            MainCanvasStateMachine.onAnimateToMenu.AddListener(OnAnimateToMenu);
        }

        private void OnDisable()
        {
            MainCanvasStateMachine.onAnimateToMenu.RemoveListener(OnAnimateToMenu);
        }

        private void OnAnimateToMenu()
        {
            buttonWindow.Open(GenericUI.DOTweenHelpers.SequenceType.UnSequenced);

            if (button != null)
                button.interactable = true;
        }
    }
}


