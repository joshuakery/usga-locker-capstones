using JoshKery.GenericUI.DOTweenHelpers.FlexibleUI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JoshKery.USGA.LockerCapstones
{
    [RequireComponent(typeof(Button))]
    public class MeetTheMembersButton : ColorChangeButton
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
            MainCanvasStateMachine.onAnimateToIntro.AddListener(OnAnimateToIntro);
        }

        private void OnDisable()
        {
            MainCanvasStateMachine.onAnimateToIntro.RemoveListener(OnAnimateToIntro);
        }

        private void OnAnimateToIntro()
        {
            buttonWindow.Open(GenericUI.DOTweenHelpers.SequenceType.UnSequenced);

            if (button != null)
                button.interactable = true;
        }
    }
}

