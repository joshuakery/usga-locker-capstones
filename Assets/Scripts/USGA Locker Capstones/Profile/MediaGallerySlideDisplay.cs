using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.GenericUI.Carousel;
using JoshKery.USGA.Directus;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class MediaGallerySlideDisplay : SlideDisplay
    {
        [SerializeField]
        private RawImageManager riManager;

        [SerializeField]
        private GameObject captionContainer;

        [SerializeField]
        private TMP_Text captionTextField;

        [SerializeField]
        private RectTransform rt;

        [SerializeField]
        private UIAnimationSequenceData outForUpNextSequence;

        [SerializeField]
        private UIAnimationSequenceData inAsUpNextSequence;

        [SerializeField]
        private UIAnimationSequenceData outForUpPrevSequence;

        [SerializeField]
        private UIAnimationSequenceData inAsUpPrevSequence;

        public void SetContent(MediaFile mediaFile)
        {
            if (mediaFile != null)
            {
                if (riManager != null)
                {
                    riManager.texture = mediaFile.texture;
                }

                if (!string.IsNullOrEmpty(mediaFile.description))
                {
                    if (captionContainer != null)
                    {
                        captionContainer.SetActive(true);
                    }
                    if (captionTextField != null)
                    {
                        captionTextField.text = mediaFile.description;
                    }
                }
                else
                {
                    if (captionContainer != null)
                    {
                        captionContainer.SetActive(false);
                    }
                }

                
            }
        }

        public override void SlideOutForNext()
        {
            rt.SetPivot(PivotPresets.MiddleLeft, false);
            base.SlideOutForNext();
        }

        public override void SlideInAsNext()
        {
            rt.SetPivot(PivotPresets.MiddleCenter, false);
            base.SlideInAsNext();
        }

        public override void SlideOutForPrev()
        {
            rt.SetPivot(PivotPresets.MiddleRight, false);
            base.SlideOutForPrev();
        }

        public override void SlideInAsPrev()
        {
            rt.SetPivot(PivotPresets.MiddleCenter, false);
            base.SlideInAsPrev();
        }

        public virtual void SlideOutForUpNext()
        {
            rt.SetPivot(PivotPresets.MiddleLeft, false);
            _WindowAction(outForUpNextSequence, SequenceType.Join);
        }

        public virtual void SlideInAsUpNext()
        {
            rt.SetPivot(PivotPresets.MiddleRight, false);
            _WindowAction(inAsUpNextSequence, SequenceType.Join);
        }

        public virtual void SlideOutForUpPrev()
        {
            rt.SetPivot(PivotPresets.MiddleRight, false);
            _WindowAction(outForUpPrevSequence, SequenceType.Join);
        }

        public virtual void SlideInAsUpPrev()
        {
            rt.SetPivot(PivotPresets.MiddleLeft, false);
            _WindowAction(inAsUpPrevSequence, SequenceType.Join);
        }
    }
}

