using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.Carousel;
using JoshKery.GenericUI.AspectRatio;

namespace JoshKery.USGA.LockerCapstones
{
    public class IntroCarouselSlideBackground : SlideDisplay
    {
        [SerializeField]
        private RawImageManager riManager;

        public void SetContent(HistorySlide historySlide)
        {
            if (historySlide != null && riManager != null)
            {
                riManager.texture = historySlide.image?.texture;
            }
        }
    }
}


