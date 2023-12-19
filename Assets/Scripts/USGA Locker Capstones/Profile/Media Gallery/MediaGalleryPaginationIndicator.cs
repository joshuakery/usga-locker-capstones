using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.Carousel;
using MagneticScrollView;
using DG.Tweening;

namespace JoshKery.USGA.LockerCapstones
{
    public class MediaGalleryPaginationIndicator : SlideDisplay
    {
        private MagneticScrollRect scrollRect;

        private Tween activeTween;

        private GameObject previouslySelected;

        private void OnSelectionChange(GameObject selected)
        {
            int siblingCount = selected.transform.parent.childCount;

            if (selected.transform.GetSiblingIndex() == transform.GetSiblingIndex())
            {
                if (previouslySelected == null)
                    StartTweenAndSetAsActiveTween(SlideInAsNext(SequenceType.CompleteImmediately));

                else if (transform.GetSiblingIndex() == 0 && previouslySelected.transform.GetSiblingIndex() == siblingCount - 1)
                    StartTweenAndSetAsActiveTween(SlideInAsPrev(SequenceType.UnSequenced));
                else if (transform.GetSiblingIndex() == siblingCount - 1 && previouslySelected.transform.GetSiblingIndex() == 0)
                    StartTweenAndSetAsActiveTween(SlideInAsNext(SequenceType.UnSequenced));

                else if (previouslySelected.transform.GetSiblingIndex() < transform.GetSiblingIndex())
                    StartTweenAndSetAsActiveTween(SlideInAsPrev(SequenceType.UnSequenced));
                else if (previouslySelected.transform.GetSiblingIndex() > transform.GetSiblingIndex())
                    StartTweenAndSetAsActiveTween(SlideInAsNext(SequenceType.UnSequenced));

            }
            else if (previouslySelected != null && previouslySelected.transform.GetSiblingIndex() == transform.GetSiblingIndex())
            {
                if (transform.GetSiblingIndex() == 0 && selected.transform.GetSiblingIndex() == siblingCount - 1)
                    StartTweenAndSetAsActiveTween(SlideOutForNext(SequenceType.UnSequenced));
                else if (transform.GetSiblingIndex() == siblingCount - 1 && selected.transform.GetSiblingIndex() == 0)
                    StartTweenAndSetAsActiveTween(SlideOutForPrev(SequenceType.UnSequenced));

                else if (transform.GetSiblingIndex() > selected.transform.GetSiblingIndex())
                    StartTweenAndSetAsActiveTween(SlideOutForNext(SequenceType.UnSequenced));
                else if (transform.GetSiblingIndex() < selected.transform.GetSiblingIndex())
                    StartTweenAndSetAsActiveTween(SlideOutForPrev(SequenceType.UnSequenced));
            }
            else
                StartTweenAndSetAsActiveTween(SlideOutForNext(SequenceType.CompleteImmediately));

            previouslySelected = selected;
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

