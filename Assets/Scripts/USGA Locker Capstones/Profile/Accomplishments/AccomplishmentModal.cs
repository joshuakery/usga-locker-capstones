using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.Text;

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
            onClose += DoClose;
            onCloseAndComplete += CloseAndComplete;

            MainCanvasStateMachine.beforeAnimateToProfile += InvokeOnCloseAndComplete;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onOpen -= SetContentAndOpen;
            onClose -= DoClose;
            onCloseAndComplete -= CloseAndComplete;

            MainCanvasStateMachine.beforeAnimateToProfile -= InvokeOnCloseAndComplete;
        }

        /// <summary>
        /// Sets the content of the card.
        /// Then waits one frame for the modal layout and for the accomplishment cards' layouts
        /// to rebuild.
        /// </summary>
        /// <param name="accomplishment"></param>

        public void SetContentAndOpen(Accomplishment accomplishment)
        {
            if (accomplishment != null)
            {
                if (headerField != null)
                {
                    headerField.text = accomplishment.name;
                    AddNoBreakTags.AddNoBreakTagsToText(headerField);
                }
                    

                if (descriptionField != null)
                {
                    descriptionField.text = accomplishment.description;
                    AddNoBreakTags.AddNoBreakTagsToText(descriptionField);
                }

                StartCoroutine(WaitThenOpen(accomplishment));
            }
            
        }

        

        /// <summary>
        /// Wait a frame for layouts to rebuild, then animate.
        /// The open animations will be INSERTED in the following order:
        /// 1. onOpening animations (i.e. the accomplishment cards' animations)
        /// 2. this modal's Open animation
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitThenOpen(Accomplishment accomplishment)
        {
            yield return null;

            if (accomplishmentCardPlaceholder != null)
            {
                onOpening?.Invoke(accomplishment, accomplishmentCardPlaceholder.position);
            }

            Open(0f);
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
            onCloseAndComplete?.Invoke();
        }

        
    }
}

