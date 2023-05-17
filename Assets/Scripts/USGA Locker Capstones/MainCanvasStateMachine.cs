using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class MainCanvasStateMachine : BaseWindow
    {
        [SerializeField]
        private UIAnimationSequenceData toAttractSequence;

        [SerializeField]
        private UIAnimationSequenceData toIntroSequence;

        [SerializeField]
        private UIAnimationSequenceData toMenuSequence;

        [SerializeField]
        private UIAnimationSequenceData toProfileSequence;

        public void AnimateToAttract()
        {
            _WindowAction(toAttractSequence, SequenceType.Join);
        }

        public void AnimateToIntro()
        {
            _WindowAction(toIntroSequence, SequenceType.Join);
        }

        public void AnimateToMenu()
        {
            _WindowAction(toMenuSequence, SequenceType.Join);
        }

        public void AnimateToProfile()
        {
            _WindowAction(toProfileSequence, SequenceType.Join);
        }
    }
}


