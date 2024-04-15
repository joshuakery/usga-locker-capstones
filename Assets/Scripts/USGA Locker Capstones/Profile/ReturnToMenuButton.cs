using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.DOTweenHelpers.FlexibleUI;
using UnityEngine.EventSystems;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    [RequireComponent(typeof(Button))]
    public class ReturnToMenuButton : ColorChangeButton
    {
        private Button button;

        private IEnumerator onClickCoroutine;

        private AccomplishmentModal accomplishmentModal;

        protected override void Awake()
        {
            base.Awake();

            button = GetComponent<Button>();

            accomplishmentModal = FindObjectOfType<AccomplishmentModal>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(OnClick);
            MainCanvasStateMachine.onAnimateToProfile.AddListener(OnAnimateToProfile);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
            MainCanvasStateMachine.onAnimateToProfile.RemoveListener(OnAnimateToProfile);
        }

        private void OnClick()
        {
            if (onClickCoroutine != null)
                return;

            onClickCoroutine = OnClickCoroutine();
            StartCoroutine(onClickCoroutine);
        }

        private IEnumerator OnClickCoroutine()
        {
            MainCanvasStateMachine.onAnimateToMenu?.Invoke();

            yield return null;

            onClickCoroutine = null;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (buttonWindow != null)
                buttonWindow.Close();
        }

        private void OnAnimateToProfile(int id)
        {
            buttonWindow.Open(SequenceType.UnSequenced);

            if (button != null)
                button.interactable = true;
        }
    }
}


