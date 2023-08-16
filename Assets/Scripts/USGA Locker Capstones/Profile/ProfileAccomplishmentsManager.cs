using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileAccomplishmentsManager : BaseDisplay
    {
        //todo dictionary to keep track of opened and closed

        public void SetContent(LockerProfile lockerProfile)
        {
            AccomplishmentModal.onClose.Invoke();

            if (lockerProfile != null)
            {
                if (lockerProfile.earnedAccomplishmentItems != null)
                {
                    ClearAllDisplays();

                    foreach (EarnedAccomplishmentItem item in lockerProfile.earnedAccomplishmentItems)
                    {
                        if (item?.earnedAccomplishment != null)
                        {
                            AccomplishmentDisplay display = InstantiateDisplay<AccomplishmentDisplay>();
                            display.SetContent(item.earnedAccomplishment);
                        }
                    }
                }
            }
        }    
    }
}


