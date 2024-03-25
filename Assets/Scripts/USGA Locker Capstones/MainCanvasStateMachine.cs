using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.Events;

namespace JoshKery.USGA.LockerCapstones
{
    public class MainCanvasStateMachine : BaseWindow
    {
        #region FIELDS
        private LockerCapstonesContentLoader contentLoader;

        [SerializeField]
        private UIAnimationSequenceData toAttractSequence;

        [SerializeField]
        private UIAnimationSequenceData toIntroSequence;

        [SerializeField]
        private UIAnimationSequenceData toMenuSequence;

        [SerializeField]
        private UIAnimationSequenceData toProfileSequence;
        #endregion

        #region UnityEvents
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

        private static IntEvent _onAnimateToProfile; 
        public static IntEvent onAnimateToProfile
        {
            get
            {
                if (_onAnimateToProfile == null)
                    _onAnimateToProfile = new IntEvent();

                return _onAnimateToProfile;
            }
        }
        #endregion

        #region Main UnityEvents Hooks
        public delegate void BeforeAnimateToMenu();
        public static BeforeAnimateToMenu beforeAnimateToMenu;

        public delegate void AfterAnimateToMenu();
        public static AfterAnimateToMenu afterAnimateToMenu;

        public delegate void BeforeAnimateToProfile();
        public static BeforeAnimateToProfile beforeAnimateToProfile;
        #endregion

        #region Monobehaviour Methods
        protected override void Awake()
        {
            base.Awake();

            contentLoader = FindObjectOfType<LockerCapstonesContentLoader>();
        }
        protected override void OnEnable()
        {
            base.OnEnable();

            onAnimateToAttract.AddListener(AnimateToAttract);
            onAnimateToIntro.AddListener(AnimateToIntro);
            onAnimateToMenu.AddListener(AnimateToMenu);
            onAnimateToProfile.AddListener(AnimateToProfile);

            if (contentLoader != null)
                contentLoader.onPopulateContentFinish.AddListener(OnPopulateContentFinish);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onAnimateToAttract.RemoveListener(AnimateToAttract);
            onAnimateToIntro.RemoveListener(AnimateToIntro);
            onAnimateToMenu.RemoveListener(AnimateToMenu);
            onAnimateToProfile.RemoveListener(AnimateToProfile);

            if (contentLoader != null)
                contentLoader.onPopulateContentFinish.RemoveListener(OnPopulateContentFinish);
        }
        #endregion

        #region Content Loader Callback
        private void OnPopulateContentFinish()
        {
            onAnimateToAttract?.Invoke();
        }
        #endregion

        #region Animate Methods
        private void AnimateToAttract()
        {
            sequenceManager.KillCurrentSequence();
            _WindowAction(toAttractSequence, SequenceType.Join);
        }

        private void AnimateToIntro()
        {
            sequenceManager.KillCurrentSequence();
            _WindowAction(toIntroSequence, SequenceType.Join);
        }

        private void AnimateToMenu()
        {
            sequenceManager.KillCurrentSequence();

            beforeAnimateToMenu?.Invoke();
            sequenceManager.CompleteCurrentSequence();
            //Reset the filters, which triggers an animation but we'll complete it immediately
            FilterDrawer.ClearFilters();
            sequenceManager.CompleteCurrentSequence();
            _WindowAction(toMenuSequence, SequenceType.Join);

            afterAnimateToMenu?.Invoke();
        }

        private void AnimateToProfile(int id)
        {
            sequenceManager.KillCurrentSequence();

            beforeAnimateToProfile?.Invoke();
            sequenceManager.CompleteCurrentSequence();
            ProfileModulesManager.onResetContent.Invoke(id);
            _WindowAction(toProfileSequence, SequenceType.Join);
        }
        #endregion

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


