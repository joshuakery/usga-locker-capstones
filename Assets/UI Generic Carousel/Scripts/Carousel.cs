using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.GenericUI.Carousel
{
    public class Carousel : MonoBehaviour
    {
        private int CurrentSlideIndex = -1;

        public SlideManager slideManager;
        public Carousel navbar;

        public bool doLoop = false;

        [SerializeField]
        private UISequenceManager sequenceManager;

        private void OnEnable()
        {
            if (slideManager != null)
                slideManager.onInitialized.AddListener(Setup);
        }

        private void OnDisable()
        {
            if (slideManager != null)
                slideManager.onInitialized.RemoveListener(Setup);
        }

        public virtual void Setup()
        {
            SlideOutAll();
            GoToFirstSlide();
        }

        private void SlideOutAll()
        {
            foreach (KeyValuePair<string, SlideDisplay> kvp in slideManager.slideDisplays)
            {
                SlideDisplay display = kvp.Value;
                display.SlideOutForPrev();
            }

            if (navbar != null)
            {
                navbar.SlideOutAll();
            }
        }

        public void GoToFirstSlide()
        {
            int _currentSlideIndex = CurrentSlideIndex;
            int firstSlideIndex = 0;

            GoToSlide(_currentSlideIndex, firstSlideIndex);
        }

        public void NextSlide()
        {
            int nextSlideIndex = (CurrentSlideIndex + 1);
            int forceDirection = 0;

            if (doLoop)
            {
                if (nextSlideIndex >= slideManager.slideDisplays.Count)
                {
                    nextSlideIndex = 0;
                    forceDirection = 1;
                }
            }

            if (nextSlideIndex >= slideManager.slideDisplays.Count) { return; }

            int _currentSlideIndex = CurrentSlideIndex;

            GoToSlide(_currentSlideIndex, nextSlideIndex, forceDirection);
        }

        public void PrevSlide()
        {
            int prevSlideIndex = (CurrentSlideIndex - 1);
            int forceDirection = 0;

            if (doLoop)
            {
                if (prevSlideIndex < 0)
                {
                    prevSlideIndex = slideManager.slideDisplays.Count - 1;
                    forceDirection = -1;
                }
            }


            if (prevSlideIndex < 0) { return; }

            int _currentSlideIndex = CurrentSlideIndex;

            GoToSlide(_currentSlideIndex, prevSlideIndex, forceDirection);
        }


        private void GoToSlide(int oldSlideIndex, int newSlideIndex, int forceDirection = 0)
        {
            if (sequenceManager != null)
                sequenceManager.CompleteCurrentSequence();

            for (int i = 0; i < slideManager.childDisplays.Count; i++)
            {
                SlideDisplay display = (SlideDisplay)slideManager.childDisplays[i];

                if (forceDirection == 1 || (forceDirection == 0 && newSlideIndex > oldSlideIndex)) //new should enter as next
                {
                    if (i == oldSlideIndex) { display.SlideOutForNext(); }
                    if (i == newSlideIndex) { display.SlideInAsNext(); }
                }
                else if (forceDirection == -1 || (forceDirection == 0 && newSlideIndex < oldSlideIndex)) //new should enter as prev
                {
                    if (i == oldSlideIndex) { display.SlideOutForPrev(); }
                    if (i == newSlideIndex) { display.SlideInAsPrev(); }
                }

            }

            if (navbar != null)
            {
                navbar.GoToSlide(oldSlideIndex, newSlideIndex, forceDirection);
            }

            CurrentSlideIndex = newSlideIndex;
        }
    }
}

