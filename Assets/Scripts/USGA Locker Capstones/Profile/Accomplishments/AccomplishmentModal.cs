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
        public delegate void OnOpen(EarnedAccomplishment earnedAccomplishment);
        public static OnOpen onOpen;

        public delegate void OnClose();
        public static OnClose onClose;

        [SerializeField]
        private TMP_Text headerField;

        [SerializeField]
        private TMP_Text descriptionField;

        [SerializeField]
        private RawImageManager iconField;

        protected override void OnEnable()
        {
            base.OnEnable();

            onOpen += SetContentAndOpen;
            onClose += Close;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onOpen -= SetContentAndOpen;
            onClose -= Close;
        }

        public void SetContentAndOpen(EarnedAccomplishment earnedAccomplishment)
        {
            if (earnedAccomplishment != null)
            {
                if (headerField != null)
                    headerField.text = earnedAccomplishment.name;

                if (descriptionField != null)
                    descriptionField.text = earnedAccomplishment.description;

                if (iconField != null)
                    iconField.texture = earnedAccomplishment.image?.texture;

                Open();
            }
            
        }

        public void InvokeOnClose()
        {
            onClose.Invoke();
        }

        
    }
}

