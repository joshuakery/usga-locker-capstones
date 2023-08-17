using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.Carousel;

namespace JoshKery.USGA.LockerCapstones
{
    public class MediaGalleryManager : SlideManager
    {
        [SerializeField]
        private Carousel carousel;

        [SerializeField]
        private GameObject navigationContainer;

        public void SetContent(LockerProfile lockerProfile)
        {
            if (lockerProfile != null)
            {
                if (lockerProfile.media != null)
                {
                    ClearAllDisplays();
                    if (navbarManager != null) { navbarManager.ClearAllDisplays(); }

                    foreach (MediaItem item in lockerProfile.media)
                    {
                        if (item != null)
                        {
                            MediaGallerySlideDisplay display = InstantiateDisplay<MediaGallerySlideDisplay>();
                            display.SetContent(item.mediaFile);

                            string slideID = item.mediaFile.filename_disk;
                            slideDisplays[slideID] = display;
                            slideOrder.Add(slideID);

                            if (navbarManager != null)
                            {
                                SlideDisplay indicator = navbarManager.InstantiateDisplay<SlideDisplay>();
                                navbarManager.slideDisplays[slideID] = indicator;
                                navbarManager.slideOrder.Add(slideID);
                            }
                        }
                    }

                    if (navigationContainer != null)
                    {
                        navigationContainer.SetActive(lockerProfile.media.Count > 1);
                    }

                    onInitialized.Invoke();
                }
            }
        }
    }
}


