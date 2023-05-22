using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        public void SetContent()
        {
            lockerNumberField.text = "#001";
            firstNameField.text = "Jack";
            lastNameField.text = "Nicklaus";
            yearInductedField.text = "Inducted 1999";
        }
    }
}


