using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.DOTweenHelpers;
using MagneticScrollView;

namespace JoshKery.USGA.LockerCapstones
{
    public class MediaGalleryPaginationManager : BaseWindow
    {
        [SerializeField]
        private MagneticScrollRect scrollRect;

        public void SetContent(LockerProfile lockerProfile)
        {
            ClearAllDisplays();

            foreach (MediaItem item in lockerProfile.media)
            {
                if (item != null)
                {
                    MediaGalleryPaginationIndicator display = InstantiateDisplay<MediaGalleryPaginationIndicator>();
                    display.SetUp(scrollRect);
                }
            }
        }
    }
}

