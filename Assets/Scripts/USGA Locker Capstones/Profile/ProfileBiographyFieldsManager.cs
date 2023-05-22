using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileBiographyFieldsManager : MonoBehaviour
    {
        [SerializeField]
        private RawImage lockerMapImage;

        [SerializeField]
        private RawImage imageField1;

        [SerializeField]
        private RawImage imageField2;

        [SerializeField]
        private TMP_Text bodyTextField;

        public void SetContent()
        {
            if (bodyTextField != null)
                bodyTextField.text = "Some filler text.";
        }
    }
}

