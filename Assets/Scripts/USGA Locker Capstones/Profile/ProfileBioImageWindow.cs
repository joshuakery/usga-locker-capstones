using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.USGA.Directus;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileBioImageWindow : BaseWindow
    {
        [SerializeField]
        private RawImageManager riManager;

        [SerializeField]
        private TMP_Text captionTextField;
        public void SetContent(MediaFile mediaFile)
        {
            if (mediaFile != null)
            {
                if (riManager != null)
                    riManager.texture = mediaFile.texture;

                if (captionTextField != null)
                    captionTextField.text = mediaFile.description;
            }
        }
    }
}


