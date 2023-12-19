using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.GenericUI.Carousel
{
    public class SlideDisplay : BaseWindow
    {
        [SerializeField]
        private UIAnimationSequenceData outForNextSequence;

        [SerializeField]
        private UIAnimationSequenceData inAsNextSequence;

        [SerializeField]
        private UIAnimationSequenceData outForPrevSequence;

        [SerializeField]
        private UIAnimationSequenceData inAsPrevSequence;

        public virtual Tween SlideOutForNext()
        {
            return _WindowAction(outForNextSequence, SequenceType.Join);
        }

        public virtual Tween SlideOutForNext(SequenceType sequenceType)
        {
            return _WindowAction(outForNextSequence, sequenceType);
        }

        public virtual Tween SlideInAsNext()
        {
            return _WindowAction(inAsNextSequence, SequenceType.Join);
        }

        public virtual Tween SlideInAsNext(SequenceType sequenceType)
        {
            return _WindowAction(inAsNextSequence, sequenceType);
        }

        public virtual Tween SlideOutForPrev()
        {
            return _WindowAction(outForPrevSequence, SequenceType.Join);
        }

        public virtual Tween SlideOutForPrev(SequenceType sequenceType)
        {
            return _WindowAction(outForPrevSequence, sequenceType);
        }

        public virtual Tween SlideInAsPrev()
        {
            return _WindowAction(inAsPrevSequence, SequenceType.Join);
        }

        public virtual Tween SlideInAsPrev(SequenceType sequenceType)
        {
            return _WindowAction(inAsPrevSequence, sequenceType);
        }
    }
}


