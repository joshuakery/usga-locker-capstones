using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JoshKery.USGA.Directus;


namespace JoshKery.USGA.LockerCapstones
{
    public class MenuItem : LockerCapstonesWindow
    {
        public List<string> categories;

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
        private TMP_Text firstNameTextField;

        [SerializeField]
        private TMP_Text lastNameTextField;

        [SerializeField]
        private TMP_Text yearInductedTextField;

        [SerializeField]
        private RawImage rawImage;

        public void SetContent(LockerProfile lockerProfile)
        {
            if (lockerProfile != null)
            {
                if (firstNameTextField != null)
                {
                    firstNameTextField.text = lockerProfile.firstName;
                }

                if (lastNameTextField != null)
                {
                    lastNameTextField.text = lockerProfile.lastName;
                }

                if (yearInductedTextField != null)
                {
                    yearInductedTextField.text = lockerProfile.inductionYear.ToString();
                }

                if (rawImage != null)
                {
                    rawImage.texture = lockerProfile.featuredImage.texture;
                }
            }


        }

        public override void SetContent()
        {

        }
    }
}


