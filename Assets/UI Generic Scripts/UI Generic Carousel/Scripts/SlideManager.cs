using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JoshKery.GenericUI.Carousel
{
    public class SlideManager : BaseDisplay
    {
        private Dictionary<string, SlideDisplay> _slideDisplays;
        public Dictionary<string, SlideDisplay> slideDisplays
        {
            get
            {
                if (_slideDisplays == null)
                    _slideDisplays = new Dictionary<string, SlideDisplay>();

                return _slideDisplays;
            }
            set
            {
                slideDisplays = value;
            }
        }

        private List<string> _slideOrder;
        public List<string> slideOrder
        {
            get
            {
                if (_slideOrder == null)
                    _slideOrder = new List<string>();

                return _slideOrder;
            }
            set
            {
                _slideOrder = value;
            }
        }

        [SerializeField]
        protected SlideManager navbarManager;

        public UnityEvent onInitialized;

        public void InstantiateSlideDisplays(int n)
        {
            ClearAllDisplays();

            for(int i=0; i<n; i++)
            {
                SlideDisplay display = InstantiateDisplay<SlideDisplay>();
                slideDisplays[i.ToString()] = display;
                slideOrder.Add(i.ToString());
            }

            if (navbarManager != null)
            {
                navbarManager.ClearAllDisplays();
                navbarManager.InstantiateSlideDisplays(n);
            }

            onInitialized.Invoke();
        }

        public override void ClearAllDisplays()
        {
            base.ClearAllDisplays();

            slideDisplays.Clear();
            slideOrder.Clear();
        }

        /// <summary>
        /// Gets an id string from slideOrder
        /// </summary>
        /// <param name="currentSlideIndex">Index of id to get</param>
        /// <returns></returns>
        public string GetCurrentSlideID(int currentSlideIndex)
        {
            if (currentSlideIndex >= 0 && currentSlideIndex < slideOrder.Count)
            {
                return slideOrder[currentSlideIndex];
            }
            else
            {
                return null;
            }
        }


    }
}


