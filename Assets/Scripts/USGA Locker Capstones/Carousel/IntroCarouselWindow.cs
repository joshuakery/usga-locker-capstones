using UnityEngine;

namespace JoshKery.USGA.LockerCapstones
{
    public class IntroCarouselWindow : LockerCapstonesWindow
    {
        [SerializeField]
        private IntroCarouselSlideManager slideManager;

        public override void SetContent()
        {
            if (appState == null) { return; }

            slideManager.SetContent(appState?.data?.era?.historySlides);
        }
    }
}

