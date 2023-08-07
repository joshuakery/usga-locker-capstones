using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.AspectRatio;

namespace JoshKery.USGA.LockerCapstones
{
    public class AccomplishmentDisplay : BaseWindow
    {
        [SerializeField]
        private AppState appState;

        [SerializeField]
        private RawImageManager iconRI;

        [SerializeField]
        private TMP_Text titleTextField;

        [SerializeField]
        private TMP_Text numberTextField;

        public Button infoButton;

        public GameObject infoIcon;

        public void SetContent(EarnedAccomplishment earnedAccomplishment)
        {
            Accomplishment accomplishmentDefault = null;
            if (appState != null && appState.accomplishmentsDict.ContainsKey(earnedAccomplishment.type.id))
            {
                accomplishmentDefault = appState.accomplishmentsDict[earnedAccomplishment.type.id];
            }

            if (earnedAccomplishment != null)
            {
                if (iconRI != null)
                {
                    if (earnedAccomplishment.customImage != null)
                    {
                        iconRI.texture = earnedAccomplishment.customImage.texture;
                    }
                    else
                    {
                        if (appState != null && appState.accomplishmentsDict.ContainsKey(earnedAccomplishment.type.id))
                        {
                            iconRI.texture = accomplishmentDefault?.image?.texture;
                        }
                    }
                }


                if (titleTextField != null)
                {
                    titleTextField.text = accomplishmentDefault?.name;
                }

                if (numberTextField != null)
                {
                    numberTextField.text = earnedAccomplishment.timesEarned.ToString() + "X";
                }
            }

        }

    }
}


