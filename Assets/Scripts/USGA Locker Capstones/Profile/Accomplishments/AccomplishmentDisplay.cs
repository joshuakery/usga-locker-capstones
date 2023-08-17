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
        private RawImageManager iconRIManager;

        [SerializeField]
        private TMP_Text titleTextField;

        [SerializeField]
        private Button infoButton;

        [SerializeField]
        private GameObject infoSkin;

        public void SetContent(EarnedAccomplishment earnedAccomplishment)
        {
            if (earnedAccomplishment != null)
            {
                if (iconRIManager != null)
                    iconRIManager.texture = earnedAccomplishment.image?.texture;

                if (titleTextField != null)
                    titleTextField.text = earnedAccomplishment.name;

                SetupInfoButton(earnedAccomplishment);
            }
        }

        private void SetupInfoButton(EarnedAccomplishment earnedAccomplishment)
        {
            infoSkin.SetActive(earnedAccomplishment.hasInfo);
            infoButton.enabled = earnedAccomplishment.hasInfo;

            infoButton.onClick.RemoveAllListeners();
            infoButton.onClick.AddListener(() =>
                {
                    AccomplishmentModal.onOpen.Invoke(earnedAccomplishment);
                }
            );
        }

    }
}


