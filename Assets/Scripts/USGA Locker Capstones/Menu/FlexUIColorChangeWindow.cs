using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace JoshKery.GenericUI.DOTweenHelpers.FlexibleUI
{
    public class FlexUIColorChangeWindow : BaseWindow
    {
        /// <summary>
        /// Creates a tween for animating a graphic's color using a FlexibleUIData's parameters
        /// </summary>
        /// <param name="tweenData">Data to work from, including to & from values and duration</param>
        /// <returns></returns>
        private static Tween Color(UIAnimationIndividualSequencingData tweenData)
        {
            if (tweenData.objectToAnimate.GetComponent<Graphic>() == null)
                tweenData.objectToAnimate.AddComponent<Graphic>();

            Graphic graphic = tweenData.objectToAnimate.GetComponent<Graphic>();

            FlexUIColorChangeAnimationData animationData = (FlexUIColorChangeAnimationData)tweenData.animationData;

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
        private static Tween VertexColor(UIAnimationIndividualSequencingData tweenData)
        {
            if (tweenData.objectToAnimate.GetComponent<TMP_Text>() == null)
                tweenData.objectToAnimate.AddComponent<TMP_Text>();

            TMP_Text tmp_text = tweenData.objectToAnimate.GetComponent<TMP_Text>();

            FlexUIColorChangeAnimationData animationData = (FlexUIColorChangeAnimationData)tweenData.animationData;

            Tween _tween = null;

            if (!animationData.useFrom)
                _tween = tmp_text.DOColor(animationData.toColor, animationData.duration);
            else
            {
                Color32 aux = tmp_text.color;

                tmp_text.color = animationData.toColor; //set to .to.x as the reference for .From()
                _tween = tmp_text.DOColor(animationData.fromColor, animationData.duration).From(); //sets alpha to .from.x

                tmp_text.color = aux; //set it back to what it was
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

        public override Tween CreateTweenFromAnimationData(UIAnimationIndividualSequencingData tweenData)
        {
            if (tweenData.animationData == null) { return null; }

            FlexUIColorChangeAnimationData animationData = (FlexUIColorChangeAnimationData)tweenData.animationData;

            switch (animationData.extendedAnimationType)
            {
                case FlexUIColorChangeAnimationData.ExtendedAnimationType.Ignore:
                    return base.CreateTweenFromAnimationData(tweenData);
                case FlexUIColorChangeAnimationData.ExtendedAnimationType.Color:
                    return Color(tweenData);
                case FlexUIColorChangeAnimationData.ExtendedAnimationType.VertexColor:
                    return VertexColor(tweenData);
                default:
                    return null;
            }
        }
    }
}


