using UnityEngine;
using TMPro;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.Carousel;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileContainerFooterNext : BaseWindow
    {
        [SerializeField]
        private Carousel carousel;

        [SerializeField]
        private TMP_Text label;

        private ProfileModuleType nextModuleType = ProfileModuleType.None;

        #region Monobehaviour Methods
        protected override void OnEnable()
        {
            base.OnEnable();

            carousel.onSlideChanged.AddListener(OnSlideChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            carousel.onSlideChanged.RemoveListener(OnSlideChanged);
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

            if (newSlideIndex < carousel.slideManager.slideDisplays.Count - 1)
            {
                Tween pulseUp = _WindowAction(openSequence, SequenceType.UnSequenced);

                pulseSequence.Append(pulseUp);
            }
        }
        #endregion

        private void OnSlideChanged(int newSlideIndex)
        {
            Pulse(newSlideIndex);
        }

        public void UpdateLabel(int newSlideIndex)
        {
            if (label != null)
            {
                if (newSlideIndex + 1 >= 0 && newSlideIndex + 1 < ProfileModulesOrder.order.Length)
                    nextModuleType = ProfileModulesOrder.order[newSlideIndex + 1];

                if (ProfileModulesOrder.moduleInfo.ContainsKey(nextModuleType))
                    label.text = ProfileModulesOrder.moduleInfo[nextModuleType];
                else
                    label.text = "NONE";
            }
        }
    }
}


