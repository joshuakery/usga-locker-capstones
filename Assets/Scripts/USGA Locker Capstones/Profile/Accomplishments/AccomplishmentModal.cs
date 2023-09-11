using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.AspectRatio;

namespace JoshKery.USGA.LockerCapstones
{
    public class AccomplishmentModal : BaseWindow
    {
        public delegate void OnOpen(Accomplishment accomplishment);
        public static OnOpen onOpen;

        public delegate void OnOpening(Accomplishment accomplishment, Vector2 cardDestination);
        /// <summary>
        /// For passing the placeholder's position to an accomplishment card for its dynamic animation to that position.
        /// </summary>
        public static OnOpening onOpening;

        public delegate void OnClose();
        public static OnClose onClose;

        public delegate void OnCloseAndComplete();
        public static OnCloseAndComplete onCloseAndComplete;

        [SerializeField]
        private TMP_Text headerField;

        [SerializeField]
        private TMP_Text descriptionField;

        [SerializeField]
        private RectTransform accomplishmentCardPlaceholder;

        protected override void OnEnable()
        {
            base.OnEnable();

            onOpen += SetContentAndOpen;
            onClose += Close;
            onCloseAndComplete += CloseAndComplete;

            MainCanvasStateMachine.beforeAnimateToProfile += InvokeOnCloseAndComplete;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onOpen -= SetContentAndOpen;
            onClose -= Close;
            onCloseAndComplete -= CloseAndComplete;

            MainCanvasStateMachine.beforeAnimateToProfile -= InvokeOnCloseAndComplete;
        }

        public void SetContentAndOpen(Accomplishment accomplishment)
        {
            if (accomplishment != null)
            {
                if (headerField != null)
                    headerField.text = accomplishment.name;

                if (descriptionField != null)
                    descriptionField.text = accomplishment.description;

                Open();

                StartCoroutine(WaitAndInvokeOnOpening(accomplishment));
            }
            
        }

        /// <summary>
        /// Wait a frame for layout to rebuild.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitAndInvokeOnOpening(Accomplishment accomplishment)
        {
            yield return null;

            if (accomplishmentCardPlaceholder != null)
            {
                onOpening.Invoke(accomplishment, accomplishmentCardPlaceholder.position);
            }
                
        }

        public void InvokeOnClose()
        {
            onClose.Invoke();
        }

        private void CloseAndComplete()
        {
            Close(SequenceType.CompleteImmediately);
        }

        private void InvokeOnCloseAndComplete()
        {
            onCloseAndComplete.Invoke();
        }

        
    }
}

