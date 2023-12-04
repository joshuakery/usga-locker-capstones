using System.Collections;
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
        private void Pulse(int newSlideIndex)
        {
            if (pulseSequence != null)
                pulseSequence.Complete();
            
            pulseSequence = DOTween.Sequence();

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


