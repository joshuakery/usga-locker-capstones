using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.GenericUI.Carousel;
using JoshKery.USGA.Directus;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class MediaGallerySlideDisplay : BaseWindow
    {
        [SerializeField]
        private RawImageManager riManager;

        public string caption { get; private set; }

        public void SetContent(MediaFile mediaFile)
        {
            if (mediaFile?.texture != null)
            {
                if (riManager != null)
                {
                    riManager.texture = mediaFile.texture;
                }

                caption = mediaFile.description;
            }
        }

        public void RebuildLayout()
        {
            if (riManager != null)
                riManager.texture = riManager.texture;
        }
    }
}

