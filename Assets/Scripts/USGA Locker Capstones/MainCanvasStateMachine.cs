using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class MainCanvasStateMachine : BaseWindow
    {
        private static UnityEvent _onAnimateToAttract;
        public static UnityEvent onAnimateToAttract
        {
            get
            {
                if (_onAnimateToAttract == null)
                    _onAnimateToAttract = new UnityEvent();

                return _onAnimateToAttract;
            }
        }

        private static UnityEvent _onAnimateToIntro;
        public static UnityEvent onAnimateToIntro
        {
            get
            {
                if (_onAnimateToIntro == null)
                    _onAnimateToIntro = new UnityEvent();

                return _onAnimateToIntro;
            }
        }

        private static UnityEvent _onAnimateToMenu;
        public static UnityEvent onAnimateToMenu
        {
            get
            {
                if (_onAnimateToMenu == null)
                    _onAnimateToMenu = new UnityEvent();

                return _onAnimateToMenu;
            }
        }

        private static UnityEvent _onAnimateToProfile; 
        public static UnityEvent onAnimateToProfile
        {
            get
            {
                if (_onAnimateToProfile == null)
                    _onAnimateToProfile = new UnityEvent();

                return _onAnimateToProfile;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            onAnimateToAttract.AddListener(AnimateToAttract);
            onAnimateToIntro.AddListener(AnimateToIntro);
            onAnimateToMenu.AddListener(AnimateToMenu);
            onAnimateToProfile.AddListener(AnimateToProfile);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onAnimateToAttract.RemoveListener(AnimateToAttract);

            onAnimateToIntro.RemoveListener(AnimateToIntro);

            onAnimateToMenu.RemoveListener(AnimateToMenu);

            onAnimateToProfile.RemoveListener(AnimateToProfile);
        }

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

        public void InvokeOnAnimateToIntro()
        {
            onAnimateToIntro.Invoke();
        }

        public void InvokeOnAnimateToMenu()
        {
            onAnimateToMenu.Invoke();
        }
    }
}


