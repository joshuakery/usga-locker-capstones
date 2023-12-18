using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using JoshH.UI;

namespace JoshKery.GenericUI.DOTweenHelpers.UIGradient
{
    public class UIGradientWindow : BaseWindow
    {
        /// <summary>
        /// Creates a tween for animating a graphic's color using a FlexibleUIData's parameters
        /// </summary>
        /// <param name="tweenData">Data to work from, including to & from values and duration</param>
        /// <returns></returns>
        private static Tween Linear(UIAnimationIndividualSequencingData tweenData)
        {
            if (tweenData.objectToAnimate.GetComponent<Graphic>() == null)
                tweenData.objectToAnimate.AddComponent<Graphic>();

            Graphic graphic = tweenData.objectToAnimate.GetComponent<Graphic>();

            UIAnimationDataUIGradient animationData = (UIAnimationDataUIGradient)tweenData.animationData;

            Tween _tween = null;

            if (!animationData.useFrom)
                _tween = graphic.DOColor(animationData.toColor, animationData.duration);
            else
            {
                Color32 aux = graphic.color;

                graphic.color = animationData.toColor; //set to .to.x as the reference for .From()
                _tween = graphic.DOColor(animationData.fromColor, animationData.duration).From(); //sets alpha to .from.x

                graphic.color = aux; //set it back to what it was
            }

            if (animationData.delay > 0)
                _tween.SetDelay(animationData.delay);

            UIAnimatorModule.SetEase(_tween, animationData);

            _tween.SetLoops(animationData.loops, animationData.loopType);

            _tween.OnStart(() =>
            {
                if (tweenData.onStartEvent != null)
                    tweenData.onStartEvent.Invoke();
            });

            _tween.OnComplete(() =>
            {
                if (tweenData.onCompleteEvent != null)
                    tweenData.onCompleteEvent.Invoke();
            });

            return _tween;
        }

        /// <summary>
        /// Creates a tween for animating a TMP_Text's vertex color using a FlexibleUIData's parameters
        /// </summary>
        /// <param name="tweenData">Data to work from, including to & from values and duration</param>
        /// <returns></returns>
        private static Tween ComplexLinear(UIAnimationIndividualSequencingData tweenData)
        {
            if (tweenData.objectToAnimate.GetComponent<JoshH.UI.UIGradient>() == null)
                tweenData.objectToAnimate.AddComponent<JoshH.UI.UIGradient>();

            JoshH.UI.UIGradient uiGradient = tweenData.objectToAnimate.GetComponent<JoshH.UI.UIGradient>();

            UIAnimationDataUIGradient animationData = (UIAnimationDataUIGradient)tweenData.animationData;

            Tween _tween = null;

            /*            if (!animationData.useFrom)
                            _tween = uiGradient.DOColor(animationData.toColor, animationData.duration);
                        else
                        {
                            Color32 aux = uiGradient.color;

                            uiGradient.color = animationData.toColor; //set to .to.x as the reference for .From()
                            _tween = uiGradient.DOColor(animationData.fromColor, animationData.duration).From(); //sets alpha to .from.x

                            uiGradient.color = aux; //set it back to what it was
                        }*/

            if (animationData.delay > 0)
                _tween.SetDelay(animationData.delay);

            UIAnimatorModule.SetEase(_tween, animationData);

            _tween.SetLoops(animationData.loops, animationData.loopType);

            _tween.OnStart(() =>
            {
                if (tweenData.onStartEvent != null)
                    tweenData.onStartEvent.Invoke();
            });

            _tween.OnComplete(() =>
            {
                if (tweenData.onCompleteEvent != null)
                    tweenData.onCompleteEvent.Invoke();
            });

            return _tween;
        }

        public override Tween CreateTweenFromAnimationData(UIAnimationIndividualSequencingData tweenData)
        {
            if (tweenData.animationData == null) { return null; }

            UIAnimationDataUIGradient animationData = (UIAnimationDataUIGradient)tweenData.animationData;

            switch (animationData.extendedAnimationType)
            {
                case UIAnimationDataUIGradient.ExtendedAnimationType.Ignore:
                    return base.CreateTweenFromAnimationData(tweenData);
                case UIAnimationDataUIGradient.ExtendedAnimationType.Linear:
                    //return Linear(tweenData);
                    return null;
                case UIAnimationDataUIGradient.ExtendedAnimationType.ComplexLinear:
                    return ComplexLinear(tweenData);
                default:
                    return null;
            }
        }
    }
}


