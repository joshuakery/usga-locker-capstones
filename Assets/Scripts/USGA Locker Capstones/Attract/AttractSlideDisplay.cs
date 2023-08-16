using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.GenericUI.Carousel;

namespace JoshKery.USGA.LockerCapstones
{
    public class AttractSlideDisplay : SlideDisplay
    {
        [SerializeField]
        private RawImageManager riManager;

        public void SetContent(Texture2D tex)
        {
            if (riManager != null)
            {
                riManager.texture = tex;
            }
        }
    }
}


