using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.AspectRatio;

namespace JoshKery.USGA.LockerCapstones
{
    public class AccomplishmentDisplay : BaseWindow
    {
        [SerializeField]
        private AppState appState;

        /// <summary>
        /// EarnedAccomplishment.id associated with this AccomplishmentDisplay
        /// </summary>
        private int id;

        [SerializeField]
        private RawImageManager iconRIManager;

        [SerializeField]
        private TMP_Text titleTextField;

        /// <summary>
        /// Button that triggers opening of AccomplishmentModal
        /// </summary>
        [SerializeField]
        private Button infoButton;

        /// <summary>
        /// Container for style elements for an enabled infoButton
        /// </summary>
        [SerializeField]
        private GameObject infoSkin;

        #region Dynamic Animation Fields
        /// <summary>
        /// Top Canvas to which the animating cards will parent to during their animations
        /// </summary>
        private GameObject topCanvas;

        /// <summary>
        /// Parent for rt to reset to after dynamic animation
        /// </summary>
        private Transform originalParent;

        /// <summary>
        /// RectTransform of card in dynamic animation
        /// </summary>
        [SerializeField]
        private RectTransform rt;

        [SerializeField]
        private UIAnimationData animationData;

        private Tween activeTween;
        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();

            AccomplishmentModal.onOpening += OnModalOpen;
            AccomplishmentModal.onClose += OnModalClose;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            AccomplishmentModal.onOpening -= OnModalOpen;
            AccomplishmentModal.onClose -= OnModalClose;
        }

        private void Start()
        {
            if (rt != null)
            {
                originalParent = rt.parent;
            }

            topCanvas = GameObject.FindGameObjectWithTag("TopCanvas");
        }

        #region Set Content
        public void SetContent(EarnedAccomplishment earnedAccomplishment)
        {
            if (earnedAccomplishment != null)
            {
                id = earnedAccomplishment.id;

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

        #endregion

        private void OnModalOpen(EarnedAccomplishment earnedAccomplishment, Vector2 destination)
        {
            
            if (id == earnedAccomplishment.id)
            {
                if (animationData != null)
                {
                    if (rt != null && topCanvas != null)
                    {
                        originalParent = rt.parent;

                        rt.parent = topCanvas.transform;

                        activeTween = rt.DOMove(destination, animationData.duration);
                        activeTween.SetDelay(animationData.delay); //todo append to sequence used by modal
                        UIAnimatorModule.SetEase(activeTween, animationData);
                    }
                }

                
            }
        }

        private void OnModalClose()
        {
            if (rt != null && originalParent != null && rt.parent != originalParent)
            {
                if (animationData != null)
                {
                    activeTween = rt.DOMove(originalParent.position, animationData.duration);
                    UIAnimatorModule.SetEase(activeTween, animationData);
                    activeTween.onComplete = () =>
                    {
                        if (originalParent != null)
                            rt.parent = originalParent;
                    };
                }

            }
        }

    }
}


