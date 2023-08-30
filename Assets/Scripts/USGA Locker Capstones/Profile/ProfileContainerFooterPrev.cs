using UnityEngine;
using TMPro;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.Carousel;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileContainerFooterPrev : BaseWindow
    {
        [SerializeField]
        private Carousel carousel;

        [SerializeField]
        private TMP_Text label;

        private ProfileModuleType prevModuleType = ProfileModuleType.None;

        #region Monobehaviour Methods
        protected override void OnEnable()
        {
            base.OnEnable();

            carousel.onSlideChanged.AddListener(OnSlideChanged);

            ProfileModulesManager.onResetContent.AddListener(Setup);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            carousel.onSlideChanged.RemoveListener(OnSlideChanged);

            ProfileModulesManager.onResetContent.RemoveListener(Setup);
        }
        #endregion

        #region Pulse Animation Method
        private void Pulse(int newSlideIndex)
        {
            Sequence pulseSequence = DOTween.Sequence();

            Tween pulseDown = _WindowAction(closeSequence, SequenceType.UnSequenced);

            pulseDown.onComplete = () =>
            {
                UpdateLabel(newSlideIndex);
            };

            pulseSequence.Join(pulseDown);

            if (newSlideIndex > 0)
            {
                Tween pulseUp = _WindowAction(openSequence, SequenceType.UnSequenced);

                pulseSequence.Append(pulseUp);
            }
        }
        #endregion

        private void Setup(int profileID)
        {
            Pulse(0);
        }

        private void OnSlideChanged(int newSlideIndex)
        {
            Pulse(newSlideIndex);
        }

        public void UpdateLabel(int newSlideIndex)
        {
            if (label != null)
            {
                if (newSlideIndex - 1 >= 0 && newSlideIndex - 1 < ProfileModulesOrder.order.Length)
                    prevModuleType = ProfileModulesOrder.order[newSlideIndex - 1];

                if (ProfileModulesOrder.moduleInfo.ContainsKey(prevModuleType))
                    label.text = ProfileModulesOrder.moduleInfo[prevModuleType];
                else
                    label.text = "NONE";
            }
        }
    }
}


