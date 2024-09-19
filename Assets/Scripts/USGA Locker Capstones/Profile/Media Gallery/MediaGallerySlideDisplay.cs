using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.GenericUI.Carousel;
using JoshKery.USGA.Directus;
using JoshKery.GenericUI.DOTweenHelpers;
using MagneticScrollView;
using DG.Tweening;

namespace JoshKery.USGA.LockerCapstones
{
    public class MediaGallerySlideDisplay : BaseWindow
    {
        [SerializeField]
        private RawImageManager riManager;

        [SerializeField]
        private GameObject videoPlayerContainer;

        [SerializeField]
        private UnityEngine.Video.VideoPlayer videoPlayer;

        public string caption { get; private set; }

        public MagneticScrollRect controllingScrollRect;

        private Tween videoPlayerAudioTween;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (controllingScrollRect != null)
            {
                controllingScrollRect.onSelectionChange.AddListener(OnSelectionChange);
            }

            MainCanvasStateMachine.onAnimateToAttract.AddListener(PauseVideo);
            MainCanvasStateMachine.onAnimateToIntro.AddListener(PauseVideo);
            MainCanvasStateMachine.onAnimateToMenu.AddListener(PauseVideo);
                
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (controllingScrollRect != null)
            {
                controllingScrollRect.onSelectionChange.RemoveListener(OnSelectionChange);
            }

            MainCanvasStateMachine.onAnimateToAttract.RemoveListener(PauseVideo);
            MainCanvasStateMachine.onAnimateToIntro.RemoveListener(PauseVideo);
            MainCanvasStateMachine.onAnimateToMenu.RemoveListener(PauseVideo);
        }

        private void OnSelectionChange(GameObject selection)
        {
            if (selection == gameObject)
                ResetVideo();
            else
                PauseVideo();
        }

        private void ResetVideo()
        {
            StartCoroutine(ResetVideoCoroutine());
        }

        /// <summary>
        /// Because setting the frame to 0 doesn't seem to work unless the videoPlayer has Played once
        /// </summary>
        /// <returns></returns>
        private IEnumerator ResetVideoCoroutine()
        {
            if (videoPlayer != null)
            {
                videoPlayer.SetDirectAudioVolume(0, 0);
                videoPlayer.Play();

                yield return null;

                videoPlayer.SetDirectAudioVolume(0, 1);
                videoPlayer.Pause();
                videoPlayer.frame = 0;
            }
        }

        private void PauseVideo()
        {
            if (videoPlayer != null)
            {
                if (videoPlayerAudioTween != null)
                    videoPlayerAudioTween.Kill();

                //rolldown volume
                videoPlayerAudioTween = DOTween.To(
                    () => videoPlayer.GetDirectAudioVolume(0),
                    x => videoPlayer.SetDirectAudioVolume(0, x),
                    0,
                    0.5f
                    );

                //do pause
                videoPlayerAudioTween.OnComplete(() =>
               {
                   videoPlayer.Pause();
                   videoPlayerAudioTween = null;
               });
            }
        }

        public void SetContent(MediaFile mediaFile, MagneticScrollRect scrollRect)
        {
            controllingScrollRect = scrollRect;
            controllingScrollRect.onSelectionChange.AddListener(OnSelectionChange);

            MainCanvasStateMachine.onAnimateToAttract.RemoveListener(PauseVideo);
            MainCanvasStateMachine.onAnimateToIntro.RemoveListener(PauseVideo);
            MainCanvasStateMachine.onAnimateToMenu.RemoveListener(PauseVideo);
            MainCanvasStateMachine.onAnimateToAttract.AddListener(PauseVideo);
            MainCanvasStateMachine.onAnimateToIntro.AddListener(PauseVideo);
            MainCanvasStateMachine.onAnimateToMenu.AddListener(PauseVideo);

            if (riManager == null) return;
            if (videoPlayerContainer == null || videoPlayer == null) return;

            riManager.gameObject.SetActive(mediaFile.HasImage());
            videoPlayerContainer.SetActive(mediaFile.HasVideoExtension());

            if (mediaFile.HasImage())
                riManager.texture = mediaFile.texture;
            else if (mediaFile.HasVideoExtension())
            {
                videoPlayer.url = mediaFile.local_path;
                ResetVideo();
            }

            caption = mediaFile.description;
        }

        public void RebuildLayout()
        {
            if (riManager != null)
                riManager.texture = riManager.texture;
        }
    }
}

