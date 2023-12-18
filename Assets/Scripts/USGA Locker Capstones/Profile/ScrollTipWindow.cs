using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class ScrollTipWindow : BaseWindow
    {
        private Tween activeTween;

        [SerializeField]
        private VerticalScrollSnap scrollSnap;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (scrollSnap != null)
                scrollSnap.OnSelectionChangeEndEvent.AddListener(OnSelectionChangeEnd);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (scrollSnap != null)
                scrollSnap.OnSelectionChangeEndEvent.RemoveListener(OnSelectionChangeEnd);
        }

        /// <summary>
        /// Closes window if on the last index, otherwise opens it
        /// Subscribes to scroll snap's OnSelectionChangeEndEvent
        /// The indices of the scroll snap are in reverse order because of the component can't automatically reverse them
        /// </summary>
        /// <param name="index"></param>
        private void OnSelectionChangeEnd(int index)
        {
            if (activeTween != null)
            {
                activeTween.Kill();
                activeTween = null;
            }

            Sequence wrapper = DOTween.Sequence();

            if (index == 0)
            {
                wrapper.Join(_Close(SequenceType.UnSequenced));
            }
            else
            {
                wrapper.Join(_Open(SequenceType.UnSequenced));
            }

            wrapper.OnComplete(() => { activeTween = null; });

            activeTween = wrapper;
        }    
    }
}


