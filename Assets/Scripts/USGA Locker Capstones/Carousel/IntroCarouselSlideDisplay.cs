using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.GenericUI.Carousel;

namespace JoshKery.USGA.LockerCapstones
{
    public class IntroCarouselSlideDisplay : SlideDisplay
    {
        [SerializeField]
        private TMP_Text titleField;

        [SerializeField]
        private TMP_Text bodyField;

        public void SetContent(HistorySlide historySlide)
        {
            if (historySlide != null)
            {
                if (titleField != null)
                {
                    titleField.text = historySlide.title;
                }

                if (bodyField != null)
                {
                    bodyField.text = historySlide.text;
                } 
            }
        }
    }
}

