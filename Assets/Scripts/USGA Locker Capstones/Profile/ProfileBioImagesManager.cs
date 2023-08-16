using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileBioImagesManager : BaseDisplay
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
                        ProfileBioImageWindow display = InstantiateDisplay<ProfileBioImageWindow>();
                        display.SetContent(mediaItem.mediaFile);
                    }
                }
            }
        }
}
}


