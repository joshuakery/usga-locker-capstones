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
        private Transform originalParentOfAnimatedRT;

        /// <summary>
        /// RectTransform of card in dynamic animation
        /// </summary>
        [SerializeField]
        private RectTransform animatedRT;

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
            if (animatedRT != null)
            {
                originalParentOfAnimatedRT = animatedRT.parent;
            }

            topCanvas = GameObject.FindGameObjectWithTag("TopCanvas");
        }

        #region Set Content
        public void SetContent(Accomplishment accomplishment)
        {
            if (accomplishment != null)
            {
                id = accomplishment.id;

                if (iconRIManager != null &&
                    appState?.data != null &&
                    accomplishment.icon != null &&
                    appState.data.accomplishmentIconsDict.ContainsKey(accomplishment.icon.id)
                    )
                {
                    iconRIManager.texture = appState.data.accomplishmentIconsDict[accomplishment.icon.id].image?.texture;
                }
                    

                if (titleTextField != null)
                    titleTextField.text = accomplishment.name;

                SetupInfoButton(accomplishment);
            }
        }

        private void SetupInfoButton(Accomplishment accomplishment)
        {
            infoSkin.SetActive(accomplishment.hasInfo);
            infoButton.enabled = accomplishment.hasInfo;

            infoButton.onClick.RemoveAllListeners();
            infoButton.onClick.AddListener(() =>
                {
                    AccomplishmentModal.onOpen.Invoke(accomplishment);
                }
            );
        }

        #endregion

        private void OnModalOpen(Accomplishment accomplishment, Vector2 destination)
        {
            
            if (id == accomplishment.id)
            {
                if (animationData != null)
                {
                    if (animatedRT != null && topCanvas != null)
                    {
                        originalParentOfAnimatedRT = animatedRT.parent;

                        animatedRT.parent = topCanvas.transform;

                        activeTween = animatedRT.DOMove(destination, animationData.duration);
                        activeTween.SetDelay(animationData.delay); //todo append to sequence used by modal
                        UIAnimatorModule.SetEase(activeTween, animationData);
                    }
                }

                
            }
        }

        private void OnModalClose()
        {
            if (animatedRT != null && originalParentOfAnimatedRT != null && animatedRT.parent != originalParentOfAnimatedRT)
            {
                if (animationData != null)
                {
                    activeTween = animatedRT.DOMove(originalParentOfAnimatedRT.position, animationData.duration);
                    UIAnimatorModule.SetEase(activeTween, animationData);
                    activeTween.onComplete = () =>
                    {
                        if (originalParentOfAnimatedRT != null)
                            animatedRT.parent = originalParentOfAnimatedRT;
                    };
                }

            }
        }

        private void OnDestroy()
        {
            if (animatedRT != null)
                Destroy(animatedRT.gameObject);
        }

    }
}


