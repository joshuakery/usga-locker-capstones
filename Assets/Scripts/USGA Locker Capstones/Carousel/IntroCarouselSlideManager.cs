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

        public void SetContent(List<HistorySlide> historySlides)
        {
            if (historySlides != null)
            {
                ClearAllDisplays();
                if (navbarManager != null) { navbarManager.ClearAllDisplays(); }

                foreach (HistorySlide slide in historySlides)
                {
                    if (slide != null)
                    {
                        IntroCarouselSlideDisplay display = InstantiateDisplay<IntroCarouselSlideDisplay>();
                        display.SetContent(slide);

                        string slideID = slide.id.ToString();
                        slideDisplays[slideID] = display;
                        slideOrder.Add(slideID);

                        if (navbarManager != null)
                        {
                            SlideDisplay indicator = navbarManager.InstantiateDisplay<SlideDisplay>();

                            navbarManager.slideDisplays[slideID] = indicator;
                            navbarManager.slideOrder.Add(slideID);
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


