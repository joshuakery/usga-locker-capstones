using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JoshKery.USGA.Directus;


namespace JoshKery.USGA.LockerCapstones
{
    public class MenuItem : LockerCapstonesWindow
    {
        private List<int> _contentTrailIDs;

        public List<int> contentTrailIDs
        {
            get
            {
                if (_contentTrailIDs == null)
                    _contentTrailIDs = new List<int>();

                return _contentTrailIDs;
            }
            set
            {
                _contentTrailIDs = value;
            }
        }

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
                contentTrailIDs.Clear();
                if (lockerProfile.contentTrailItems != null)
                {
                    contentTrailIDs = lockerProfile.contentTrailItems
                        .Where(item => (item != null && item.contentTrail != null))
                        .Select(item => item.contentTrail.id)
                        .ToList();
                    contentTrailIDs.Add(-1); //the default ALL Inductees category
                }

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


