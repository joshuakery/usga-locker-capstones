using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.DOTweenHelpers.FlexibleUI;
using UnityEngine.EventSystems;

namespace JoshKery.USGA.LockerCapstones
{
    public class LockerLocatorButton : ColorChangeButton
    {
        [SerializeField]
        private Button button;

        private AccomplishmentModal accomplishmentModal;

        private IEnumerator onClickCoroutine = null;

        private LockerLocatorDrawer lockerLocatorDrawer;

        public delegate void OnButtonClick();
        public static OnButtonClick onButtonClick;

        protected override void Awake()
        {
            base.Awake();

            accomplishmentModal = FindObjectOfType<AccomplishmentModal>();
            lockerLocatorDrawer = FindObjectOfType<LockerLocatorDrawer>();
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

        public override void OnPointerDown(PointerEventData eventData)
        {
            //do nothing
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            //do nothing
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            //do nothing
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            //do nothing
        }

        private void OnClick()
        {
            if (onClickCoroutine != null)
                return;

            SetGraphics();

            onClickCoroutine = OnClickCoroutine();
            StartCoroutine(onClickCoroutine);
        }

        private IEnumerator OnClickCoroutine()
        {
            yield return null;

            if (accomplishmentModal.isOpen)
            {
                Tween close = accomplishmentModal.Close(SequenceType.UnSequenced);
                if (close == null) yield break;
                float duration = close.Duration();
                close.Kill();
                AccomplishmentModal.onClose?.Invoke();
                yield return new WaitForSeconds(duration + 0.05f);
            }

            onButtonClick?.Invoke();

            lockerLocatorDrawer.Toggle();

            yield return null;

            onClickCoroutine = null;
        }

        public void SetGraphics()
        {
            Debug.Log("SETTING GRAPHICS FOR LOCKER LOCATOR BUTTON");

            if (buttonWindow == null) return;

            buttonWindow.sequenceManager.CompleteCurrentSequence();

            if (lockerLocatorDrawer == null) return;

            if (lockerLocatorDrawer.isOpen)
                buttonWindow.Close(SequenceType.UnSequenced);
            else
                buttonWindow.Open(SequenceType.UnSequenced);
        }

        private void AfterAnimateToMenu()
        {
            buttonWindow.Close(SequenceType.UnSequenced);
        }

        private void InvertGraphics()
        {
            if (buttonWindow == null) return;

            buttonWindow.sequenceManager.CompleteCurrentSequence();

            if (lockerLocatorDrawer == null) return;

            if (buttonWindow.isOpen)
                buttonWindow.Close(SequenceType.CompleteImmediately);
            else
                buttonWindow.Open(SequenceType.CompleteImmediately);
        }
    }
}


