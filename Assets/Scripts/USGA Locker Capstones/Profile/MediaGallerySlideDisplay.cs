using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.GenericUI.Carousel;
using JoshKery.USGA.Directus;

namespace JoshKery.USGA.LockerCapstones
{
    public class MediaGallerySlideDisplay : SlideDisplay
    {
        [SerializeField]
        private RawImageManager riManager;

        [SerializeField]
        private GameObject captionContainer;

        [SerializeField]
        private TMP_Text captionTextField;

        public void SetContent(MediaFile mediaFile)
        {
            if (mediaFile != null)
            {
                if (riManager != null)
                {
                    riManager.texture = mediaFile.texture;
                }

                if (!string.IsNullOrEmpty(mediaFile.description))
                {
                    if (captionContainer != null)
                    {
                        captionContainer.SetActive(true);
                    }
                    if (captionTextField != null)
                    {
                        captionTextField.text = mediaFile.description;
                    }
                }
                else
                {
                    if (captionContainer != null)
                    {
                        captionContainer.SetActive(false);
                    }
                }

                
            }
        }
    }
}

