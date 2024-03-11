using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.USGA.Directus;
using JoshKery.GenericUI.Text;

namespace JoshKery.USGA.LockerCapstones
{
    public class BioImageWindow : BaseWindow
    {
        [SerializeField]
        private RawImageManager riManager;

        [SerializeField]
        private TMP_Text captionTextField;

        public void SetContent(MediaFile mediaFile)
        {
            if (mediaFile?.texture != null && mediaFile.texture.width > 0 && mediaFile.texture.height > 0)
            {
                if (riManager != null)
                    riManager.texture = mediaFile.texture;

                if (captionTextField != null)
                {
                    captionTextField.text = mediaFile.description;
                    AddNoBreakTags.AddNoBreakTagsToText(captionTextField);
                }
                    
            }
        }
    }
}


