using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JoshKery.USGA.Directus;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;


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

        [SerializeField]
        private UIAnimationSequenceData specialHighlightSequence;

        [SerializeField]
        private UIAnimationSequenceData specialCloseSequence;

        protected override void OnEnable()
        {
            base.OnEnable();

            MainCanvasStateMachine.onAnimateToMenu.AddListener(OnAnimateToMenu);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            MainCanvasStateMachine.onAnimateToMenu.RemoveListener(OnAnimateToMenu);
        }

        private void OnAnimateToMenu()
        {
            if (button != null)
                button.interactable = true;
        }

        /// <summary>
        /// Does the menuItem match any of the given selectedContentTrailIDs?
        /// </summary>
        /// <param name="selectedContentTrailIDs">int IDs which would match menuItem's contentTrailIDs</param>
        /// <returns></returns>
        public bool IsInFilter(List<int> selectedContentTrailIDs)
        {
            if (selectedContentTrailIDs != null && selectedContentTrailIDs.Count > 0)
                return selectedContentTrailIDs.Intersect(contentTrailIDs).Any();
            else
                return true;
        }

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
                    yearInductedTextField.text = "Class of " + lockerProfile.inductionYear.ToString();
                }

                if (rawImage != null && lockerProfile.featuredImage != null)
                {
                    rawImage.texture = lockerProfile.featuredImage.texture;
                }
            }
        }

        public override void SetContent()
        {
            //do nothing
        }

        #region Animation Methods
        public Sequence _SpecialHighlight(
            SequenceType sequenceType = SequenceType.UnSequenced,
            float atPosition = 0f
        )
        {
            return _WindowAction(specialHighlightSequence, sequenceType, atPosition);
        }

        public Sequence _SpecialClose(
            SequenceType sequenceType = SequenceType.UnSequenced,
            float atPosition = 0f
        )
        {
            isOpen = false;
            return _WindowAction(specialCloseSequence, sequenceType, atPosition);
        }
        #endregion
    }
}


