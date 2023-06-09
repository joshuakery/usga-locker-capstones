using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.Carousel;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.USGA.Directus;

namespace JoshKery.USGA.LockerCapstones
{
    [RequireComponent(typeof(Button))]
    public class MediaGalleryIndicator : SlideDisplay
    {
        private Button _button;
        public Button button
        {
            get
            {
                if (_button == null)
                    _button = GetComponent<Button>();

                return _button;
            }
            
        }
 
        [SerializeField]
        private RawImageManager riManager;

        public void SetContent(MediaFile mediaFile)
        {
            if (mediaFile != null)
            {
                if (riManager != null)
                {
                    riManager.texture = mediaFile?.texture;
                }
            }
        }
    }
}


