using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JoshKery.GenericUI.DOTweenHelpers;
using MagneticScrollView;
using DG.Tweening;
using JoshKery.GenericUI.Text;

namespace JoshKery.USGA.LockerCapstones
{
    public class MediaGalleryCaptionWindow : BaseWindow
    {
        [SerializeField]
        private MagneticScrollRect scrollRect;

        [SerializeField]
        private TMP_Text textDisplay;

        private string currentCaptionText = "";

        private Tween activeTween;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (scrollRect != null)
                scrollRect.onSelectionChange.AddListener(OnSelectionChange);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (scrollRect != null)
                scrollRect.onSelectionChange.RemoveListener(OnSelectionChange);
        }

        private void OnSelectionChange(GameObject obj)
        {
            MediaGallerySlideDisplay slideDisplay = obj.GetComponent<MediaGallerySlideDisplay>();

            currentCaptionText = slideDisplay.caption;

            if (activeTween != null)
            {
                activeTween.Kill();
                activeTween = null;
            }
            Sequence wrapperSequence = DOTween.Sequence();
            Tween pulseTween = _Pulse(PulseType.OpenCloseOpen, SequenceType.UnSequenced);
            wrapperSequence.Join(pulseTween);
            wrapperSequence.OnComplete(() => { activeTween = null; });
            activeTween = wrapperSequence;
        }

        protected override void MidPulseCallback()
        {
            if (textDisplay != null)
            {
                textDisplay.text = currentCaptionText;
                AddNoBreakTags.AddNoBreakTagsToText(textDisplay);
                ParseItalics.ParseItalicsInText(textDisplay);
                RemoveDoubleCarriageReturns.Process(textDisplay);
            }
                
        }
    }
}


