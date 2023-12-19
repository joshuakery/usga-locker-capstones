using UnityEngine;
using JoshKery.GenericUI.Carousel;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class IntroCarouselWindow : LockerCapstonesWindow
    {
        [SerializeField]
        private IntroCarouselSlideManager slideManager;

        [SerializeField]
        private Carousel mainCarousel;

        protected override void OnEnable()
        {
            base.OnEnable();

            MainCanvasStateMachine.onAnimateToIntro.AddListener(OnAnimateToIntro);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            MainCanvasStateMachine.onAnimateToIntro.RemoveListener(OnAnimateToIntro);
        }

        private void OnAnimateToIntro()
        {
            if (mainCarousel != null)
            {
                mainCarousel.GoToFirstSlide(SequenceType.CompleteImmediately);
            }
        }

        public override void SetContent()
        {
            if (appState == null) { return; }

            slideManager.SetContent(appState?.data?.era?.historySlides);
        }
    }
}

