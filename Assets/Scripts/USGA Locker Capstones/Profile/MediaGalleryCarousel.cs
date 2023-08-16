using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.Carousel;

namespace JoshKery.USGA.LockerCapstones
{
    public class MediaGalleryCarousel : Carousel
    {
        protected override void SlideOutAll()
        {
            foreach (KeyValuePair<string, SlideDisplay> kvp in slideManager.slideDisplays)
            {
                MediaGallerySlideDisplay display = (MediaGallerySlideDisplay)kvp.Value;
                display.SlideOutForUpPrev();
            }

/*            if (navbar != null)
            {
                navbar.SlideOutAll();
            }*/
        }
        protected override void GoToSlide(
            int oldSlideIndex,
            int newSlideIndex,
            ForceDirection forceDirection = ForceDirection.Default,
            bool doCompleteCurrentSequence = true
        )
        {
            if (doCompleteCurrentSequence && sequenceManager != null)
                sequenceManager.CompleteCurrentSequence();

            for (int i = 0; i < slideManager.slideOrder.Count; i++)
            {
                string id = slideManager.slideOrder[i];
                if (slideManager.slideDisplays.ContainsKey(id))
                {
                    MediaGallerySlideDisplay display = (MediaGallerySlideDisplay)slideManager.slideDisplays[id];

                    if (forceDirection == ForceDirection.NewIsNext ||
                        (forceDirection == ForceDirection.Default && newSlideIndex > oldSlideIndex)
                    ) //new should enter as next; old should exit for next
                    {
                        if (i == oldSlideIndex && i != newSlideIndex) { display.SlideOutForNext(); }
                        if (i == newSlideIndex) { display.SlideInAsNext(); }

                        //the slide beyond the new slide, if it exists, should slide in as upnext
                        if (i == GetIndexOfNextSlide(newSlideIndex)) { display.SlideInAsUpNext(); }

                        //the slide beyond the old slide, if it exists, should slide out for the old slide to become upprev
                        if (i == GetIndexOfPrevSlide(oldSlideIndex)) { display.SlideOutForUpPrev(); }
                    }
                    else if (forceDirection == ForceDirection.NewIsPrev ||
                             (forceDirection == ForceDirection.Default && newSlideIndex < oldSlideIndex)
                    ) //new should enter as prev; old should exit for prev
                    {
                        if (i == oldSlideIndex && i != newSlideIndex) { display.SlideOutForPrev(); }
                        if (i == newSlideIndex) { display.SlideInAsPrev(); }

                        //the slide beyond the new slide, if it exists, should slide in as upprev
                        if (i == GetIndexOfPrevSlide(newSlideIndex)) { display.SlideInAsUpPrev(); }

                        //the slide beyond the old slide, if it exists, should slide out for the old slide to become upnext
                        if (i == GetIndexOfNextSlide(oldSlideIndex)) { display.SlideOutForUpNext(); }
                    }
                }
            }

            /*if (navbar != null)
            {
                navbar.GoToSlide(oldSlideIndex, newSlideIndex, forceDirection, false);
            }*/

            CurrentSlideIndex = newSlideIndex;

            onSlideChanged.Invoke(CurrentSlideIndex);
        }

        private int GetIndexOfNextSlide(int i)
        {
            if (doLoop)
                return (i + 1) % slideManager.slideDisplays.Count;
            else
                return i + 1; //if i is the index of the last slide, then the next slide will be an index out of range
        }

        private int GetIndexOfPrevSlide(int i)
        {
            if (doLoop)
            {
                if (i - 1 < 0)
                    return slideManager.slideDisplays.Count - 1;
                else
                    return i - 1;
            }
            else
                return i - 1; //if i is the index of the first slide, then the prev slide will be an index out of range
        }
    }
}


