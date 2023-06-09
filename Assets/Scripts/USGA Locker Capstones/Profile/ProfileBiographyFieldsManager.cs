using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.USGA.Directus;

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

        public void SetContent(LockerProfile lockerProfile)
        {
            if (lockerProfile != null)
            {
                if (bodyTextField != null)
                {
                    bodyTextField.text = lockerProfile.bioText;
                }

                if (imageField1 != null && lockerProfile.bioImages != null && lockerProfile.bioImages.Count >= 1)
                {
                    imageField1.texture = lockerProfile.bioImages[0].mediaFile?.texture;
                }

                if (imageField2 != null && lockerProfile.bioImages != null && lockerProfile.bioImages.Count >= 2)
                {
                    imageField2.texture = lockerProfile.bioImages[1].mediaFile?.texture;
                }

                if (lockerMapImage != null)
                {
/*                    lockerMapImage.texture = 
*/                }
            }



        }
    }
}

