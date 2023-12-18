using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class AccomplishmentsManager : BaseDisplay
    {
        [SerializeField]
        GameObject headerWithTip;

        //todo dictionary to keep track of opened and closed

        public void SetContent(LockerProfile lockerProfile)
        {
            AccomplishmentModal.onClose.Invoke();

            bool atLeastOneAchivementHasInfo = false;

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

                            if (accomplishment.hasInfo)
                                atLeastOneAchivementHasInfo = true;
                        }
                    }

                    if (headerWithTip != null)
                        headerWithTip.SetActive(atLeastOneAchivementHasInfo);
                }
            }
        }    
    }
}


