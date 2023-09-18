using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.Carousel;

namespace JoshKery.USGA.LockerCapstones
{
    public class IntroCarouselSlideManager : SlideManager
    {
        [SerializeField]
        private GameObject navigationContainer;

        [SerializeField]
        private SlideManager backgroundSlideManager;

        public void SetContent(List<HistorySlide> historySlides)
        {
            if (historySlides != null)
            {
                ClearAllDisplays();
                if (navbarManager != null) { navbarManager.ClearAllDisplays(); }
                if (backgroundSlideManager != null) { backgroundSlideManager.ClearAllDisplays(); }

                foreach (HistorySlide slideData in historySlides)
                {
                    if (slideData != null)
                    {
                        IntroCarouselSlideDisplay display = InstantiateDisplay<IntroCarouselSlideDisplay>();
                        display.SetContent(slideData);

                        string slideID = slideData.id.ToString();
                        slideDisplays[slideID] = display;
                        slideOrder.Add(slideID);

                        if (navbarManager != null)
                        {
                            SlideDisplay indicator = navbarManager.InstantiateDisplay<SlideDisplay>();

                            navbarManager.slideDisplays[slideID] = indicator;
                            navbarManager.slideOrder.Add(slideID);
                        }

                        if (backgroundSlideManager != null)
                        {
                            IntroCarouselSlideBackground backgroundSlide = backgroundSlideManager.InstantiateDisplay<IntroCarouselSlideBackground>();

                            backgroundSlideManager.slideDisplays[slideID] = backgroundSlide;
                            backgroundSlideManager.slideOrder.Add(slideID);

                            backgroundSlide.SetContent(slideData);
                        }
                    }
                }

                if (navigationContainer != null)
                {
                    navigationContainer.SetActive(slideDisplays.Count > 1);
                }

                onInitialized.Invoke();
            }
        }
    }
}


