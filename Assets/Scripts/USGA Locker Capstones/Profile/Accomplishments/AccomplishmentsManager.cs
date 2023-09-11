using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class AccomplishmentsManager : BaseDisplay
    {
        //todo dictionary to keep track of opened and closed

        public void SetContent(LockerProfile lockerProfile)
        {
            AccomplishmentModal.onClose.Invoke();

            if (lockerProfile != null)
            {
                if (lockerProfile.accomplishments != null)
                {
                    ClearAllDisplays();

                    foreach (Accomplishment accomplishment in lockerProfile.accomplishments)
                    {
                        if (accomplishment != null)
                        {
                            AccomplishmentDisplay display = InstantiateDisplay<AccomplishmentDisplay>();
                            display.SetContent(accomplishment);
                        }
                    }
                }
            }
        }    
    }
}


