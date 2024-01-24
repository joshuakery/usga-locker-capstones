using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;

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
            Sequence sequence = DOTween.Sequence();

            Tween partOne = _WindowAction(closeSequence);
            sequence.Join(partOne);

            sequence.AppendCallback(UpdateContent);

            if (condition())
            {
                Tween partTwo = _WindowAction(openSequence);
                sequence.Append(partTwo);
            }

            switch (sequenceType)
            {
                case (SequenceType.CompleteImmediately):
                    sequence.Complete();
                    break;
                case (SequenceType.UnSequenced):
                    break;
                case (SequenceType.Join):
                    sequenceManager.JoinTween(sequence);
                    break;
                case (SequenceType.Append):
                    sequenceManager.AppendTween(sequence);
                    break;
                case (SequenceType.Insert):
                    sequenceManager.InsertTween(atPosition, sequence);
                    break;
                case (SequenceType.BackInsert):
                    sequenceManager.InsertTween(sequenceManager.currentSequence.Duration() - atPosition, sequence);
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
                    selectedHeadingField.text = contentTrail.name;

                if (selectedBodyField != null)
                    selectedBodyField.text = contentTrail.description;
            }
        }
    }
}


