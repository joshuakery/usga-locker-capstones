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

        [SerializeField]
        private GameObject quoteContainer;

        [SerializeField]
        private TMP_Text quoteField;

        [SerializeField]
        private GameObject imageContainer;

        [SerializeField]
        private RawImageManager imageManager;

        public void SetContent(HistorySlide historySlide)
        {
            if (historySlide != null)
            {
                if (titleField != null)
                {
                    titleField.text = historySlide.title;
                }

                int slideType = 0;
                if (!string.IsNullOrEmpty(historySlide.text))
                    slideType = 1;
                else if (!string.IsNullOrEmpty(historySlide.quote))
                    slideType = 2;
                else if (!string.IsNullOrEmpty(historySlide.image?.filename_download))
                    slideType = 3;

                if (bodyField != null)
                {
                    bodyField.gameObject.SetActive(slideType == 1);
                    bodyField.text = historySlide.text;
                }

                if (quoteContainer != null)
                {
                    quoteContainer.SetActive(slideType == 2);
                    if (quoteField != null)
                    {
                        quoteField.text = historySlide.quote;
                    }
                }

                if (imageContainer != null)
                {
                    imageContainer.SetActive(slideType == 3);
                    if (imageManager != null)
                    {
                        imageManager.texture = historySlide.image.texture;
                    }
                }


                
            }
        }
    }
}

