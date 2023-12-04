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

        private Sequence pulseSequence;

        #region Monobehaviour Methods
        protected override void OnEnable()
        {
            base.OnEnable();

            if (carousel != null)
                carousel.onSlideChanged.AddListener(OnSlideChanged);

            ProfileModulesManager.onResetContent.AddListener(WaitAndComplete);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (carousel != null)
                carousel.onSlideChanged.RemoveListener(OnSlideChanged);

            ProfileModulesManager.onResetContent.RemoveListener(WaitAndComplete);
        }
        #endregion

        #region Pulse Animation Method
        private void Pulse(int index)
        {
            if (pulseSequence != null)
                pulseSequence.Complete();
            
            pulseSequence = DOTween.Sequence();

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

        private void WaitAndComplete(int id)
        {
            StartCoroutine(WaitAndCompleteCo());
        }

        private IEnumerator WaitAndCompleteCo()
        {
            yield return null;

            if (pulseSequence != null)
                pulseSequence.Complete();
        }
    }
}


