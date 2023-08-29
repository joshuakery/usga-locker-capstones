using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.Carousel;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileContainerHeader : BaseWindow
    {
        [SerializeField]
        private Carousel carousel;

        [SerializeField]
        private TMP_Text label;

        private ProfileModuleType currentModuleType = ProfileModuleType.Biography;

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
        private void Pulse(int index)
        {
            Sequence pulseSequence = DOTween.Sequence();

            Tween pulseDown = _WindowAction(closeSequence, SequenceType.UnSequenced);

            pulseDown.onComplete = () =>
            {
                UpdateLabel(index);
            };

            pulseSequence.Join(pulseDown);

            Tween pulseUp = _WindowAction(openSequence, SequenceType.UnSequenced);

            pulseSequence.Append(pulseUp);
        }
        #endregion

        private void OnSlideChanged(int index)
        {
            Pulse(index);
        }

        public void UpdateLabel(int index)
        {
            if (label != null)
            {
                if (index >= 0 && index < ProfileModulesOrder.order.Length)
                    currentModuleType = ProfileModulesOrder.order[index];

                if (ProfileModulesOrder.moduleInfo.ContainsKey(currentModuleType))
                    label.text = ProfileModulesOrder.moduleInfo[currentModuleType];
                else
                    label.text = "ERROR";
            }
        }
    }
}


