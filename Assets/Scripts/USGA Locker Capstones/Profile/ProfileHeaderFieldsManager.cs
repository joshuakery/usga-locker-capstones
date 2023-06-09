using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.USGA.Directus;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileHeaderFieldsManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text lockerNumberField;

        [SerializeField]
        private TMP_Text firstNameField;

        [SerializeField]
        private TMP_Text lastNameField;

        [SerializeField]
        private TMP_Text yearInductedField;

        [SerializeField]
        private RawImage profileImage;

        [SerializeField]
        private RawImage signatureImage;

        public void SetContent(LockerProfile lockerProfile)
        {
            if (lockerProfile != null)
            {
                if (lockerNumberField != null)
                    lockerNumberField.text = "#" + lockerProfile.lockerNumber.ToString();

                if (firstNameField != null)
                    firstNameField.text = lockerProfile.firstName;

                if (lastNameField != null)
                    lastNameField.text = lockerProfile.lastName;

                if (yearInductedField != null)
                    yearInductedField.text = "Inducted " + lockerProfile.inductionYear.ToString();

                if (profileImage != null)
                    profileImage.texture = lockerProfile.featuredImage.texture;

                if (signatureImage != null)
                    signatureImage.texture = lockerProfile.signatureImage.texture;
            }

        }
    }
}


