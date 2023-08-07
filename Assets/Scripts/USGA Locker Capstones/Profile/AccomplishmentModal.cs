using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.AspectRatio;

namespace JoshKery.USGA.LockerCapstones
{
    public class AccomplishmentModal : BaseWindow
    {
        [SerializeField]
        private TMP_Text headerField;

        [SerializeField]
        private TMP_Text descriptionField;

        [SerializeField]
        private RawImageManager iconField;

        public void SetContent(string header, string description, Texture2D icon)
        {
            if (headerField != null)
                headerField.text = header;

            if (descriptionField != null)
                descriptionField.text = description;

            if (iconField != null)
                iconField.texture = icon;
        }

        public void Close()
        {
            Close(SequenceType.Join);
        }
    }
}

