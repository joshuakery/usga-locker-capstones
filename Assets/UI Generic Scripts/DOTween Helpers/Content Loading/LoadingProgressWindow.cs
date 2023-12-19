using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JoshKery.GenericUI.ContentLoading;

namespace JoshKery.GenericUI.DOTweenHelpers.ContentLoading
{
    public class LoadingProgressWindow : BaseWindow
    {
        [SerializeField]
        private ContentLoader contentLoader;

        [SerializeField]
        private TMP_Text mainMessageDisplay;

        [SerializeField]
        private Slider progressSlider;

        [SerializeField]
        private TMP_Text detailsDisplay;

        [SerializeField]
        private GameObject errorContainer;

        [SerializeField]
        private TMP_Text errorDisplay;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (errorContainer != null)
                errorContainer.SetActive(false);

            contentLoader.onLoadContentCoroutineStart.AddListener(OnLoadContentCoroutineStart);
            contentLoader.onLoadingProgress += OnLoadingProgress;
            contentLoader.onLoadingDetails += OnLoadingDetails;
            contentLoader.onLoadingError += OnLoadingError;
            contentLoader.onPopulateContentFinish.AddListener(OnPopulateContentFinished);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            contentLoader.onLoadContentCoroutineStart.RemoveListener(OnLoadContentCoroutineStart);
            contentLoader.onLoadingProgress -= OnLoadingProgress;
            contentLoader.onLoadingDetails -= OnLoadingDetails;
            contentLoader.onLoadingError -= OnLoadingError;
            contentLoader.onPopulateContentFinish.RemoveListener(OnPopulateContentFinished);
        }

        private void OnLoadContentCoroutineStart()
        {
            _Open(SequenceType.UnSequenced);
        }

        private void OnLoadingProgress(string m, string d, float t, float l)
        {
            if (mainMessageDisplay != null)
                mainMessageDisplay.text = m;

            if (detailsDisplay != null)
                detailsDisplay.text = d;

            if (progressSlider != null)
            {
                if (t <= 0)
                    progressSlider.value = 0;
                else
                    progressSlider.value = l / t;
            }
                
        }

        private void OnLoadingDetails(string d)
        {
            if (detailsDisplay != null)
                detailsDisplay.text = d;
        }

        private void OnLoadingError(string e)
        {
            if (errorContainer != null)
                errorContainer.SetActive(true);

            if (errorDisplay != null)
                errorDisplay.text = e;
        }

        private void OnPopulateContentFinished()
        {
            _Close(SequenceType.UnSequenced);
        }
    }
}


