using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI;

namespace JoshKery.USGA.LockerCapstones
{
    public class BioImagesManager : BaseDisplay
    {
        public void SetContent(LockerProfile lockerProfile)
        {
            ClearAllDisplays();

            if (lockerProfile?.bioImages != null)
            {
                foreach (MediaItem mediaItem in lockerProfile.bioImages)
                {
                    if (mediaItem?.mediaFile != null)
                    {
                        BioImageWindow display = InstantiateDisplay<BioImageWindow>();
                        display.SetContent(mediaItem.mediaFile);
                    }
                }
            }
        }
}
}


