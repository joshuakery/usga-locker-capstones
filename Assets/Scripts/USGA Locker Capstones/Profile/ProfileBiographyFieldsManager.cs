using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.USGA.Directus;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileBiographyFieldsManager : BaseWindow
    {
        [SerializeField]
        private TMP_Text nameTextField;

        [SerializeField]
        private TMP_Text birthDeathInfoTextField;

        [SerializeField]
        private TMP_Text bodyTextField;

        [SerializeField]
        private GameObject quoteContainer;

        [SerializeField]
        private TMP_Text quoteTextField;

        [SerializeField]
        private TMP_Text bylineTextField;

        [SerializeField]
        private ProfileBioImagesManager bioImagesManager;

        public void SetContent(LockerProfile lockerProfile)
        {
            if (lockerProfile != null)
            {
                if (nameTextField != null)
                    nameTextField.text = lockerProfile.fullName;

                if (birthDeathInfoTextField != null)
                    birthDeathInfoTextField.text = lockerProfile.lifeDates + "\n" + lockerProfile.birthplace;

                if (bodyTextField != null)
                {
                    bodyTextField.text = lockerProfile.bioText;
                }

                if (quoteContainer != null)
                {
                    quoteContainer.SetActive( !string.IsNullOrEmpty(lockerProfile.quote) );
                }
                
                if (quoteTextField != null)
                {
                    quoteTextField.text = lockerProfile.quote;
                }

                if (bylineTextField != null)
                {
                    bylineTextField.text = lockerProfile.quoteByline;
                }

                if (bioImagesManager != null)
                    bioImagesManager.SetContent(lockerProfile);
            }



        }
    }
}

