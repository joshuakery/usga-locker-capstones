using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.DOTweenHelpers;
using MagneticScrollView;
using DG.Tweening;

namespace JoshKery.USGA.LockerCapstones
{
    public class MediaGalleryPaginationIndicator : BaseWindow
    {
        private MagneticScrollRect scrollRect;

        private Tween activeTween;

        private void OnSelectionChange(GameObject selected)
        {
            if (selected.transform.GetSiblingIndex() == transform.GetSiblingIndex())
            {
                StartTweenAndSetAsActiveTween(Open(SequenceType.UnSequenced));
            }
            else
            {
                StartTweenAndSetAsActiveTween(Close(SequenceType.UnSequenced));
            }
        }

        private void StartTweenAndSetAsActiveTween(Tween tween)
        {
            if (activeTween != null)
            {
                activeTween.Kill();
                activeTween = null;
            }

            Sequence wrapper = DOTween.Sequence();
            wrapper.Join(tween);
            wrapper.OnComplete(() => { activeTween = null; });
            activeTween = wrapper;
        }

        public void SetUp(MagneticScrollRect msr)
        {
            scrollRect = msr;

            if (scrollRect != null)
                scrollRect.onSelectionChange.AddListener(OnSelectionChange);
        }

        private void OnDestroy()
        {
            if (scrollRect != null)
                scrollRect.onSelectionChange.RemoveListener(OnSelectionChange);
        }
    }
}

