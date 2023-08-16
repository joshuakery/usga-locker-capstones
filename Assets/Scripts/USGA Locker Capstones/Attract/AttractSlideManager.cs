using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.Carousel;

namespace JoshKery.USGA.LockerCapstones
{
    public class AttractSlideManager : SlideManager
    {
        [SerializeField]
        private AppState appState;

        /// <summary>
        /// For each texture given, instantiate a slide display and set its content with that texture
        /// </summary>
        /// <param name="textures"></param>
        public void InstantiateSlideDisplays()
        {
            if (appState != null)
            {
                ClearAllDisplays();

                for (int i = 0; i < appState.attractMedia.Count; i++)
                {
                    AttractSlideDisplay display = InstantiateDisplay<AttractSlideDisplay>();
                    slideDisplays[i.ToString()] = display;
                    slideOrder.Add(i.ToString());

                    display.SetContent(appState.attractMedia[i]);
                }

                if (navbarManager != null)
                {
                    navbarManager.ClearAllDisplays();
                    navbarManager.InstantiateSlideDisplays(appState.attractMedia.Count);
                }

                onInitialized.Invoke();
            }
        }
    }

}


