using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using AirtableUnity.PX.Model;
using rlmg.logging;
using JoshKery.GenericUI.ContentLoading.Airtable;
using JoshKery.GenericUI.ContentLoading;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.GenericUI.Example
{
    public class ExampleAirtableLoader : AirtableLoader<ExampleFields>
    {
        [SerializeField]
        private ExampleMediaState mediaState;

        [SerializeField]
        private ExampleResultsDisplayManager resultsDisplayManager;

        

        protected override void OnEnable()
        {
            if (onPopulateContentFinish != null)
                onPopulateContentFinish.AddListener(InvokeOnStartUpWindows);

            base.OnEnable();
        }

        protected override void OnDisable()
        {
            if (onPopulateContentFinish != null)
                onPopulateContentFinish.RemoveListener(InvokeOnStartUpWindows);

            base.OnDisable();
        }

        //--------------------------------------Target Load Airtable--------------------------------------
        protected override IEnumerator AirtableResponseFinish(List<BaseRecord<ExampleFields>> records)
        {
            yield return StartCoroutine(LoadMedia(records));
            yield return StartCoroutine(PopulateContent(records));
            yield return StartCoroutine(base.AirtableResponseFinish(records));
        }

        //--------------------------------------Fallback Load Local--------------------------------------
        protected override IEnumerator LoadLocalContentSuccess(string contentData)
        {
            List<BaseRecord<ExampleFields>> records = new List<BaseRecord<ExampleFields>>();

            try
            {
                records = JsonConvert.DeserializeObject<List<BaseRecord<ExampleFields>>>(contentData);
            }
            catch (JsonSerializationException e)
            {
                RLMGLogger.Instance.Log(
                    String.Format("Json from file was improperly formatted. The following exception was caught: {0}", e),
                    MESSAGETYPE.ERROR
                );
            }

            if (records != null && records.Count > 0)
            {
                yield return StartCoroutine(LoadMedia(records));
                yield return StartCoroutine(PopulateContent(records));

                onPopulateContentFinish.Invoke();
            }
        }

        protected override IEnumerator LoadLocalContentFinish(UnityWebRequest.Result result)
        {
            yield return null;
        }

        //--------------------------------------Load Media Into Memory--------------------------------------
        private IEnumerator LoadMedia(List<BaseRecord<ExampleFields>> records)
        {
            if (mediaState != null)
            {
                foreach (BaseRecord<ExampleFields> record in records)
                {
                    if (!System.String.IsNullOrEmpty(record.fields.CarouselBackgroundMediaPath) &&
                        MediaLoadingUtility.IsImageFile(record.fields.CarouselBackgroundMediaPath)
                        )
                    {
                        yield return mediaState.CreateMediaFileObject(
                            record.fields.CarouselBackgroundMediaPath,
                            PrepareMediaPath(
                                record.fields.CarouselBackgroundMediaPath
                            )
                        );
                    }
                        
                }
            }

            yield return null;
        }

        private string PrepareMediaPath(string path)
        {
            path = MediaLoadingUtility.RemoveStartingPathSplitCharacter(path);
            path = Path.Combine(LocalContentDirectory, path);
            return path;
        }

        //--------------------------------------Populate Content--------------------------------------
        /// <summary>
        /// Helper function to handle records data
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private IEnumerator PopulateContent(List<BaseRecord<ExampleFields>> records)
        {
            switch (refreshType)
            {
                case (RefreshType.NoRefresh):
                    if (resultsDisplayManager != null &&
                        (resultsDisplayManager.childDisplays == null || resultsDisplayManager.childDisplays.Count == 0)
                        )
                    {
                        resultsDisplayManager.InstantiateResultsDisplays(records);
                    }
                    break;
                case (RefreshType.HardRefresh):
                    if (resultsDisplayManager != null)
                    {
                        resultsDisplayManager.ClearAllDisplays();
                        resultsDisplayManager.InstantiateResultsDisplays(records);
                    } 
                    break;
                case (RefreshType.SoftRefresh):
                    if (resultsDisplayManager != null)
                    {
                        resultsDisplayManager.RefreshResultsDisplays(records, refreshLimit);
                    }
                    break;
            }

            yield return null;
        }

        //--------------------------------------After Populate Content--------------------------------------
        private void InvokeOnStartUpWindows()
        {
            if (BaseWindow.onStartUpWindows != null)
                BaseWindow.onStartUpWindows.Invoke();
        }


        //--------------------------------------UI Input--------------------------------------
        public void OnRefreshTypeChanged(int option)
        {
            refreshType = (RefreshType)option;
        }

        public void OnRefreshLimitChanged(int option)
        {
            refreshLimit = (RefreshLimit)option;
        }

    }
}



