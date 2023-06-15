using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.USGA.Directus;
using TMPro;

namespace JoshKery.USGA.LockerCapstones
{
    public class MenuWindow : LockerCapstonesWindow
    {
        [SerializeField]
        private TMP_Text titleTextField;

        [SerializeField]
        private TMP_Text descriptionTextField;

        public override void SetContent()
        {
            if (appState == null) { return; }

            if (titleTextField != null)
            {
                titleTextField.text = appState.data?.era?.title;
            }

            if (descriptionTextField != null)
            {
                descriptionTextField.text = appState.data?.era?.description;
            }
        }
    }
}


