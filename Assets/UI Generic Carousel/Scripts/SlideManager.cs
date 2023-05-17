using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JoshKery.GenericUI.Carousel
{
    public class SlideManager : BaseDisplay
    {
        public Dictionary<string, SlideDisplay> slideDisplays;

        [SerializeField]
        private SlideManager navbarManager;

        public UnityEvent onInitialized;

        private void Start()
        {
            InstantiateSlideDisplays(3);
        }

        public void InstantiateSlideDisplays(int n)
        {
            ClearAllDisplays();

            for(int i=0; i<n; i++)
            {
                SlideDisplay display = InstantiateDisplay<SlideDisplay>();
                
                if (slideDisplays == null) { slideDisplays = new Dictionary<string, SlideDisplay>(); }

                slideDisplays[i.ToString()] = display;
            }

            if (navbarManager != null)
            {
                navbarManager.ClearAllDisplays();
                navbarManager.InstantiateSlideDisplays(n);
            }

            onInitialized.Invoke();
        }

        
    }
}


