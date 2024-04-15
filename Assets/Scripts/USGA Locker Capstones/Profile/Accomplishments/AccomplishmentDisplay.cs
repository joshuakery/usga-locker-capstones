using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.GenericUI.Text;

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

        [SerializeField]
        private Texture2D placeholderIcon;

        private bool isInModal = false;

        #region Dynamic Animation Fields
        /// <summary>
        /// Top Canvas to which the animating cards will parent to during their animations
        /// </summary>
        private GameObject topCanvas;

        /// <summary>
        /// Parent for animatedRT, the position of which animatedRT's position will be reset to
        /// after OnModalOpen dynamic animation
        /// </summary>
        private Transform originalParentOfAnimatedRT;

        /// <summary>
        /// RectTransform of card that gets dynamically animated
        /// </summary>
        [SerializeField]
        private RectTransform animatedRT;

        /// <summary>
        /// Duration, easing, etc. for dynamic OnModalOpen & OnModalClose animations
        /// </summary>
        [SerializeField]
        private UIAnimationData animationData;

        [SerializeField]
        private UIAnimationSequenceData onModalOpenAdditionalAnimations;

        [SerializeField]
        private UIAnimationSequenceData onModalCloseAdditionalAnimations;
        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();

            AccomplishmentModal.onOpening += OnModalOpen;
            AccomplishmentModal.onClose += OnModalClose;
            ModulePaginatorManager.onModulesPageChange += OnModulesPageChange;
            MainCanvasStateMachine.onAnimateToMenu.AddListener(OnAnimateToMenu);
            LockerLocatorButton.onButtonClick += OnLockerLocatorButtonClick;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            AccomplishmentModal.onOpening -= OnModalOpen;
            AccomplishmentModal.onClose -= OnModalClose;
            ModulePaginatorManager.onModulesPageChange -= OnModulesPageChange;
            MainCanvasStateMachine.onAnimateToMenu.RemoveListener(OnAnimateToMenu);
            LockerLocatorButton.onButtonClick -= OnLockerLocatorButtonClick;
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
                else
                {
                    iconRIManager.texture = placeholderIcon;
                }
                    

                if (titleTextField != null)
                {
                    titleTextField.text = accomplishment.name;
                    AddNoBreakTags.AddNoBreakTagsToText(titleTextField);
                    ParseItalics.ParseItalicsInText(titleTextField);
                }
                    

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

        /// <summary>
        /// If this accomplishment display is for the given accomplishment,
        /// Animates this display to the destination
        /// </summary>
        /// <param name="accomplishment">Data object with id to match</param>
        /// <param name="destination">Screen space position to animate to</param>
        private void OnModalOpen(Accomplishment accomplishment, Vector2 destination)
        {
            if (id == accomplishment.id)
            {
                if (isInModal) return;

                isInModal = true;

                Tween additionalAnimationsTween = _WindowAction(onModalOpenAdditionalAnimations);
                sequenceManager.JoinTween(additionalAnimationsTween);

                if (animationData != null)
                {
                    if (animatedRT != null && topCanvas != null)
                    {
                        originalParentOfAnimatedRT = animatedRT.parent;

                        animatedRT.parent = topCanvas.transform;

                        Tween positionTween = animatedRT.DOMove(destination, animationData.duration);
                        /*positionTween.SetDelay(animationData.delay); //todo append to sequence used by modal*/
                        UIAnimatorModule.SetEase(positionTween, animationData);

                        sequenceManager.InsertTween(animationData.delay, positionTween);
                    }
                }
            }
        }

        /// <summary>
        /// If this accomplishment display is not attached to its original parent (i.e. the Accomplishments Container),
        /// Animates this display back to its original position
        /// </summary>
        private void OnModalClose()
        {
            if (animatedRT != null && originalParentOfAnimatedRT != null && animatedRT.parent != originalParentOfAnimatedRT)
            {
                if (!isInModal) return;

                isInModal = false;

                Tween additionalAnimationsTween = _WindowAction(onModalCloseAdditionalAnimations);
                sequenceManager.JoinTween(additionalAnimationsTween);

                if (animationData != null)
                {
                    Tween positionTween = animatedRT.DOMove(originalParentOfAnimatedRT.position, animationData.duration);
                    UIAnimatorModule.SetEase(positionTween, animationData);
                    positionTween.onComplete = () =>
                    {
                        if (originalParentOfAnimatedRT != null)
                            animatedRT.parent = originalParentOfAnimatedRT;
                    };

                    sequenceManager.JoinTween(positionTween);
                }
            }
        }

        private void OnModulesPageChange(int page)
        {
            ResetPosition();
        }

        private void OnAnimateToMenu()
        {
            //ResetPosition();
        }

        private void OnLockerLocatorButtonClick()
        {
            ResetPosition();
        }

        /// <summary>
        /// Resets the accomplishments to their default should the page change interrupt the animation
        /// </summary>
        private void ResetPosition()
        {
            if (animatedRT != null)
            {
                if (originalParentOfAnimatedRT != null)
                    animatedRT.parent = originalParentOfAnimatedRT;

                animatedRT.localPosition = new Vector2(0, 0);
            }
        }

        private void OnDestroy()
        {
            if (animatedRT != null)
                Destroy(animatedRT.gameObject);
        }

    }
}


