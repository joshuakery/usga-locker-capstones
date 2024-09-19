using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.Text;
using DG.Tweening;

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

/*        [SerializeField]
        private BaseWindow topCanvasWindow;

        [SerializeField]
        private BaseWindow profileWindow;*/

        private void Start()
        {
            CloseAndComplete();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            onOpen += SetContentAndOpen;
            onClose += DoClose;
            onCloseAndComplete += CloseAndComplete;

            MainCanvasStateMachine.beforeAnimateToProfile += InvokeOnCloseAndComplete;
            //MainCanvasStateMachine.onAnimateToProfile.AddListener(OnAnimateToProfile);
            MainCanvasStateMachine.afterAnimateToMenu += OnAfterAnimateToMenu;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onOpen -= SetContentAndOpen;
            onClose -= DoClose;
            onCloseAndComplete -= CloseAndComplete;

            MainCanvasStateMachine.beforeAnimateToProfile -= InvokeOnCloseAndComplete;
            //MainCanvasStateMachine.onAnimateToProfile.RemoveListener(OnAnimateToProfile);
            MainCanvasStateMachine.afterAnimateToMenu -= OnAfterAnimateToMenu;
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
                    ParseItalics.ParseItalicsInText(headerField);
                    RemoveDoubleCarriageReturns.Process(headerField);
                }
                    

                if (descriptionField != null)
                {
                    descriptionField.text = accomplishment.description;
                    AddNoBreakTags.AddNoBreakTagsToText(descriptionField);
                    ParseItalics.ParseItalicsInText(descriptionField);
                    RemoveDoubleCarriageReturns.Process(descriptionField);
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

        private void OnAfterAnimateToMenu()
        {
            DoClose();
        }

/*        private void OnAnimateToProfile(int profileID)
        {
            if (topCanvasWindow != null)
            {
                topCanvasWindow.Open(SequenceType.CompleteImmediately);
            }
        }*/

 /*       protected override Sequence _Close(SequenceType sequenceType = SequenceType.UnSequenced, float atPosition = 0)
        {
            Sequence wrapper = DOTween.Sequence();

            Tween close = base._Close(SequenceType.UnSequenced, atPosition);

            if (close != null)
                wrapper.Join(close);

            if (topCanvasWindow != null && profileWindow != null && !profileWindow.isOpen)
            {
                Tween topCanvasClose = topCanvasWindow.Close(SequenceType.UnSequenced);

                if (topCanvasClose != null)
                    wrapper.Join(topCanvasClose);
            }

            sequenceManager.CreateSequenceIfNull();
            AttachTweenToSequence(sequenceType, wrapper, sequenceManager.currentSequence, false, atPosition, null);

            return wrapper;
        }

        protected override Sequence _Open(SequenceType sequenceType = SequenceType.UnSequenced, float atPosition = 0)
        {
            Sequence wrapper = DOTween.Sequence();

            Tween open = base._Open(SequenceType.UnSequenced, atPosition);

            if (open != null)
                wrapper.Join(open);

            if (topCanvasWindow != null)
            {
                Tween topCanvasOpen = topCanvasWindow.Open(SequenceType.UnSequenced);

                if (topCanvasOpen != null)
                    wrapper.Join(topCanvasOpen);
            }

            sequenceManager.CreateSequenceIfNull();
            AttachTweenToSequence(sequenceType, wrapper, sequenceManager.currentSequence, false, atPosition, null);

            return wrapper;
        }*/


    }
}

