using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.GenericUI.Carousel;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class AttractSlideDisplay : SlideDisplay
    {
        [SerializeField]
        private RawImageManager riManager;

        [SerializeField]
        private RectTransform toZoom;

        [SerializeField]
        private float zoomTarget = 1.333f;

        [SerializeField]
        private float zoomDuration = 40f;

        private Tween zoomTween;

        protected override void OnEnable()
        {
            base.OnEnable();

            MainCanvasStateMachine.onAnimateToAttract.AddListener(OnAnimateToAttract);
            MainCanvasStateMachine.onAnimateToIntro.AddListener(OnAnimateToIntro);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            MainCanvasStateMachine.onAnimateToAttract.RemoveListener(OnAnimateToAttract);
            MainCanvasStateMachine.onAnimateToIntro.RemoveListener(OnAnimateToIntro);
        }

        private void OnAnimateToAttract()
        {
            if (zoomTween != null)
            {
                zoomTween.Kill();
                zoomTween = null;
            }

            if (toZoom != null)
            {
                zoomTween = toZoom.DOScale(new Vector3(zoomTarget, zoomTarget, zoomTarget), zoomDuration);
            }
        }

        private void OnAnimateToIntro()
        {
            if (zoomTween != null)
            {
                zoomTween.Kill();
                zoomTween = null;
            }

            if (toZoom != null)
            {
                zoomTween = toZoom.DOScale(new Vector3(1 , 1, 1), 0.75f);
                zoomTween.SetEase(Ease.InOutQuad);
            }
        }

        public void SetContent(Texture2D tex)
        {
            if (riManager != null)
            {
                riManager.texture = tex;
            }
        }

        private void StartZoom()
        {
            if (zoomTween != null)
            {
                zoomTween.Kill();
                zoomTween = null;
            }

            if (toZoom != null)
            {
                toZoom.localScale = new Vector3(1, 1, 1);
                zoomTween = toZoom.DOScale( new Vector3(zoomTarget, zoomTarget, zoomTarget), zoomDuration );
            }
                
        }

        public override Tween SlideInAsPrev(SequenceType sequenceType)
        {
            StartZoom();
            return base.SlideInAsPrev(sequenceType);
        }

        public override Tween SlideInAsNext(SequenceType sequenceType)
        {
            StartZoom();
            return base.SlideInAsNext();
        }
    }
}


