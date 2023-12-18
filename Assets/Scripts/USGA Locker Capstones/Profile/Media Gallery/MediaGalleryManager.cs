using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using JoshKery.GenericUI.DOTweenHelpers;
using MagneticScrollView;

namespace JoshKery.USGA.LockerCapstones
{
    public class MediaGalleryManager : BaseWindow
    {
        [SerializeField]
        private MagneticScrollRect scrollRect;

        [SerializeField]
        private RectTransform swipeDetectionArea;

        [SerializeField]
        private MediaGalleryPaginationManager paginationManager;

        public void SetContent(LockerProfile lockerProfile)
        {
            if (lockerProfile != null)
            {
                if (lockerProfile.media != null)
                {
                    if (paginationManager != null)
                        paginationManager.SetContent(lockerProfile);

                    StartCoroutine(SetContentCoroutine(lockerProfile));
                }
            }
        }

        public IEnumerator SetContentCoroutine(LockerProfile lockerProfile)
        {
            if (scrollRect == null) { yield break; }

            scrollRect.StartAutoArranging();

            yield return null;

            base.ClearAllDisplays();

            yield return null;

            foreach (MediaItem item in lockerProfile.media)
            {
                if (item != null)
                {
                    MediaGallerySlideDisplay display = InstantiateDisplay<MediaGallerySlideDisplay>();
                    display.SetContent(item.mediaFile);
                }
            }

            yield return null;

            scrollRect.StopAutoArranging();

            MagneticScrollView.SwipeDetection swipeDetection = scrollRect.gameObject.GetComponent<MagneticScrollView.SwipeDetection>();
            swipeDetection.scrollRect = swipeDetectionArea;
        }

        public override void ClearAllDisplays()
        {
            return;
        }

    }
}


