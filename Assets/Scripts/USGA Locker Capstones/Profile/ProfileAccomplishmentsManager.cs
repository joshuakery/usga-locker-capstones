using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileAccomplishmentsManager : BaseDisplay
    {
        [SerializeField]
        private AppState appState;

        //todo dictionary to keep track of opened and closed

        [SerializeField]
        private AccomplishmentModal accomplishmentsModal;

        public void SetContent(LockerProfile lockerProfile)
        {
            if (accomplishmentsModal != null)
                accomplishmentsModal.Close();

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

                            #region Button Logic
                            Accomplishment accomplishmentDefault = null;
                            if (appState != null && appState.accomplishmentsDict.ContainsKey(earnedAccomplishment.type.id))
                            {
                                accomplishmentDefault = appState.accomplishmentsDict[earnedAccomplishment.type.id];
                            }

                            if (!string.IsNullOrEmpty(earnedAccomplishment.customDescription) ||
                                !string.IsNullOrEmpty(accomplishmentDefault.description))
                            {
                                display.infoIcon.SetActive(true);
                                display.infoButton.enabled = true;

                                string header = accomplishmentDefault?.name;
                                string description = !string.IsNullOrEmpty(earnedAccomplishment?.customDescription) ?
                                    earnedAccomplishment.customDescription :
                                    accomplishmentDefault?.description;
                                Texture2D icon = (earnedAccomplishment != null) && (earnedAccomplishment.customImage != null) ?
                                    earnedAccomplishment.customImage?.texture :
                                    accomplishmentDefault?.image?.texture;

                                display.infoButton.onClick.AddListener(() =>
                               {
                                   if (accomplishmentsModal != null)
                                   {
                                       accomplishmentsModal.SetContent(header, description, icon);
                                       accomplishmentsModal.Open(SequenceType.Join);
                                   }
                               });
                            }
                            else
                            {
                                display.infoIcon.gameObject.SetActive(false);
                                display.infoButton.enabled = false;
                            }
                            #endregion
                        }
                    }

                    
                }
            }
        }    
    }
}


