using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileAccomplishmentsManager : BaseDisplay
    {
        public void SetContent(LockerProfile lockerProfile)
        {
            if (lockerProfile != null)
            {
                if (lockerProfile.earnedAccomplishmentItems != null)
                {
                    ClearAllDisplays();

                    foreach (EarnedAccomplishmentItem item in lockerProfile.earnedAccomplishmentItems)
                    {
                        if (item?.earnedAccomplishment != null)
                        {
                            EarnedAccomplishment earnedAccomplishment = item.earnedAccomplishment;
                            AccomplishmentDisplay display = InstantiateDisplay<AccomplishmentDisplay>();
                            display.SetContent(earnedAccomplishment);
                        }
                    }
                }
            }
        }    
    }
}


