using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.DOTweenHelpers.FlexibleUI;
using UnityEngine.EventSystems;

namespace JoshKery.USGA.LockerCapstones
{
    [RequireComponent(typeof(Button))]
    public class ReturnToMenuButton : ColorChangeButton
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
            MainCanvasStateMachine.onAnimateToProfile.AddListener(OnAnimateToProfile);
        }

        private void OnDisable()
        {
            MainCanvasStateMachine.onAnimateToProfile.RemoveListener(OnAnimateToProfile);
        }

        private void OnAnimateToProfile(int id)
        {
            buttonWindow.Open(GenericUI.DOTweenHelpers.SequenceType.UnSequenced);

            if (button != null)
                button.interactable = true;
        }
    }
}


