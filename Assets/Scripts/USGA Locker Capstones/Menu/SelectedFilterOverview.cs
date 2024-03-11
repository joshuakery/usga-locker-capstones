using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.Text;

namespace JoshKery.USGA.LockerCapstones
{
    public class SelectedFilterOverview : LockerCapstonesWindow
    {
        [SerializeField]
        private TMP_Text selectedHeadingField;

        [SerializeField]
        private TMP_Text selectedBodyField;

        private int selectedContentTrailID = -1;

        #region Monobehaviour Methods
        protected override void OnEnable()
        {
            base.OnEnable();

            FilterDrawer.onFilterClicked += SetSelectedContentTrailAndUpdateContent;
            MainCanvasStateMachine.onAnimateToProfile.AddListener(OnAnimateToProfile);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            FilterDrawer.onFilterClicked -= SetSelectedContentTrailAndUpdateContent;
            MainCanvasStateMachine.onAnimateToProfile.RemoveListener(OnAnimateToProfile);
        }
        #endregion

        private void OnAnimateToProfile(int id)
        {
            _Close(SequenceType.UnSequenced);
        }

        #region Pulse Animation Methods
        /// <summary>
        /// Creates a pulse sequence of one or two tweens.
        /// Part one is the close tween.
        /// Part two is the open tween.
        /// If the given condition returns true, the second tween is created and appended to the sequence.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="sequenceType"></param>
        /// <param name="atPosition"></param>
        protected virtual void _ConditionalPulse(
            System.Func<bool> condition,
            SequenceType sequenceType = SequenceType.UnSequenced,
            float atPosition = 0f
        )
        {
            Sequence wrapper = DOTween.Sequence();

            if (isOpen)
            {
                Tween partOne = _Close(SequenceType.UnSequenced);
                wrapper.Join(partOne);
            }

            wrapper.AppendCallback(UpdateContent);

            if (condition())
            {
                Tween partTwo = _Open(SequenceType.UnSequenced);
                wrapper.Append(partTwo);
            }

            switch (sequenceType)
            {
                case (SequenceType.CompleteImmediately):
                    wrapper.Complete();
                    break;
                case (SequenceType.UnSequenced):
                    break;
                case (SequenceType.Join):
                    sequenceManager.JoinTween(wrapper);
                    break;
                case (SequenceType.Append):
                    sequenceManager.AppendTween(wrapper);
                    break;
                case (SequenceType.Insert):
                    sequenceManager.InsertTween(atPosition, wrapper);
                    break;
                case (SequenceType.BackInsert):
                    sequenceManager.InsertTween(sequenceManager.currentSequence.Duration() - atPosition, wrapper);
                    break;
            }
        }

        public virtual void ConditionalPulse(System.Func<bool> condition, SequenceType sequenceType)
        {
            _ConditionalPulse(condition, sequenceType);
        }

        public virtual void ConditionalPulse(System.Func<bool> condition, float atPosition)
        {
            _ConditionalPulse(condition, SequenceType.Insert, atPosition);
        }
        #endregion

        /// <summary>
        /// Subscribes to FilterDrawer onFilterSelected
        /// </summary>
        /// <param name="contentTrailID"></param>
        private void SetSelectedContentTrailAndUpdateContent(int contentTrailID, SequenceType sequenceType)
        {
            SetSelectedContentTrail(contentTrailID);
            ConditionalPulse(
                () => {
                    return selectedContentTrailID >= 0;
                },
                sequenceType
            );
        }

        private void SetSelectedContentTrailAndUpdateContent(int contentTrailID)
        {
            SetSelectedContentTrailAndUpdateContent(contentTrailID, SequenceType.Join);
        }

        private void SetSelectedContentTrail(int contentTrailID)
        {
            selectedContentTrailID = contentTrailID;
        }

        public void UpdateContent()
        {
            if (appState?.data?.contentTrailsDict != null && appState.data.contentTrailsDict.ContainsKey(selectedContentTrailID))
            {
                SetContent(appState.data.contentTrailsDict[selectedContentTrailID]);
            }
        }

        public void SetContent(ContentTrail contentTrail)
        {
            if (contentTrail != null)
            {
                if (selectedHeadingField != null)
                {
                    selectedHeadingField.text = contentTrail.name;
                    AddNoBreakTags.AddNoBreakTagsToText(selectedHeadingField);
                }
                    

                if (selectedBodyField != null)
                {
                    selectedBodyField.text = contentTrail.description;
                    AddNoBreakTags.AddNoBreakTagsToText(selectedBodyField);
                }
                    

            }
        }
    }
}


