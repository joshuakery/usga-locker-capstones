using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using JoshKery.GenericUI.ContentLoading;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.USGA.Directus;
using GraphQlClient.Core;
using Newtonsoft.Json;
using rlmg.logging;
using JoshKery.GenericUI.GraphQL;

namespace JoshKery.USGA.LockerCapstones
{
    public class LockerCapstonesContentLoader : ContentLoader
    {
        #region FIELDS
        [SerializeField]
        private UISequenceManager sequenceManager;

        public string graphQLURL;
        public string authToken;
        public string operationName;

        [Multiline]
        public string query;

        #region GraphQL Variables
        public int erasByIdIdVariable = 0;
        #endregion

        [SerializeField]
        private string OnlineContentDirectory;

        [SerializeField]
        private AppState appState;

        [SerializeField]
        private GraphApi erasByIdGraphRef;
        #endregion

        #region Monobehaviour Methods


        private void OnEnable()
        {
            if (onPopulateContentFinish != null)
            {
                onPopulateContentFinish.AddListener(InvokeDOTweenHelpersStartUp);
                onPopulateContentFinish.AddListener(ReportMissingLockerLocatorMedia);
            }
                
        }

        private void OnDisable()
        {
            if (onPopulateContentFinish != null)
            {
                onPopulateContentFinish.RemoveListener(InvokeDOTweenHelpersStartUp);
                onPopulateContentFinish.RemoveListener(ReportMissingLockerLocatorMedia);
            }
                
        }
        #endregion

        #region Graph Loading Methods
        /// <summary>
        /// Override method that gets called via LoadContent()
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator LoadTargetContent()
        {
            yield return LoadGraphContent().AsIEnumerator();
        }

        private async Task LoadGraphContent()
        {
            object variables = new { erasByIdId = erasByIdIdVariable };

            UnityWebRequest request = await HttpHandler.PostAsync(graphQLURL, query, variables, operationName, authToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                GraphResponseFail(request);
            }
            else
            {
                await GraphResponseSuccess(request.downloadHandler.text);
            }
        }

        private async Task GraphResponseSuccess(string text)
        {
            Debug.Log(text);

            SaveContentFileToDisk(text);

            await PopulateContent(text);

            onPopulateContentFinish.Invoke();

            await new WaitForEndOfFrame(); //TODO why do I do this?
        }

        private void GraphResponseFail(UnityWebRequest request)
        {
            RLMGLogger.Instance.Log(request.error, MESSAGETYPE.ERROR);
            //TODO UI display of error handling and option to try again
        }
        #endregion

        #region Load Local Content Fallback
        protected override IEnumerator LoadLocalContentSuccess(string text)
        {
            yield return StartCoroutine(PopulateContent(text));

            SaveContentFileToDisk(text);

            onPopulateContentFinish.Invoke();

            yield return new WaitForEndOfFrame();
        }

        protected override IEnumerator LoadLocalContentFinish(UnityWebRequest.Result result)
        {
            yield return null;
        }
        #endregion

        #region Save Local Content to Disk Override
        public override void SaveContentFileToDisk(string text)
        {
            string aux = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<LockerCapstonesDataWrapper>(text), Formatting.Indented);

            base.SaveContentFileToDisk(aux);
        }
        #endregion



        #region PopulateContent
        private IEnumerator PopulateContent(string text)
        {
            yield return null;
            //TODO manage displays for progess updates?

            LockerCapstonesDataWrapper wrapper = JsonConvert.DeserializeObject<LockerCapstonesDataWrapper>(text);
            appState.data = wrapper.data;

            yield return StartCoroutine(LoadLockerCapstonesMedia());

            yield return null;

            LockerCapstonesWindow.onSetContent.Invoke();
            LockerCapstonesStateMachine.onSetContent.Invoke();
            FilterButtonsManager.onSetContent.Invoke();
            
        }

        private IEnumerator LoadLockerCapstonesMedia()
        {
            if (appState?.data == null) { yield break; }

            // AccomplishmentIcons must come first in order to assign images to the locker profiles' accomplishments
            if (appState.data.accomplishmentIcons != null)
            {
                foreach (AccomplishmentIcon icon in appState.data.accomplishmentIcons)
                {
                    yield return StartCoroutine(LoadMediaForAccomplishmentIcon(icon));
                }
            }

            if (appState.data.lockerProfiles != null)
            {
                foreach (LockerProfile lockerProfile in appState.data.lockerProfiles)
                {
                    yield return StartCoroutine(LoadMediaForLockerProfile(lockerProfile));
                }
            }
            
            if (appState.data.era != null)
            {
                yield return StartCoroutine(LoadMediaForEra(appState.data.era));
            }
        }

        private IEnumerator LoadMediaForLockerProfile(LockerProfile lockerProfile)
        {
            if (lockerProfile != null)
            {
                yield return StartCoroutine(LoadMediaFromMediaFile(lockerProfile.featuredImage));
                
                if (lockerProfile.media != null)
                {
                    foreach (MediaItem item in lockerProfile.media)
                    {
                        yield return StartCoroutine(LoadMediaFromMediaFile(item?.mediaFile));
                    }
                }

                yield return StartCoroutine(LoadMediaFromMediaFile(lockerProfile.signatureImage));

                if (lockerProfile.bioImages != null)
                {
                    foreach (MediaItem item in lockerProfile.bioImages)
                    {
                        yield return StartCoroutine(LoadMediaFromMediaFile(item?.mediaFile));
                    }
                }
            }
        }

        private IEnumerator LoadMediaForAccomplishmentIcon(AccomplishmentIcon icon)
        {
            yield return StartCoroutine(LoadMediaFromMediaFile(icon?.image));
        }

        private IEnumerator LoadMediaForEra(Era era)
        {
            if (era?.historySlides != null)
            {
                foreach (HistorySlide slide in era.historySlides)
                {
                    yield return StartCoroutine(LoadMediaFromMediaFile(slide?.image));
                    yield return StartCoroutine(LoadMediaFromMediaFile(slide?.backgroundVideo));
                }
            }
        }

        private IEnumerator LoadMediaFromMediaFile(MediaFile mediaFile)
        {
            if (mediaFile != null)
            {
                string localPath = GetLocalMediaPath(mediaFile.filename_disk);

                if (File.Exists(localPath))
                {
                    yield return StartCoroutine(appState.SetMediaFileTextureFromPath(mediaFile, localPath));
                }
                else if (!doDefaultLocalLoadContent)
                {
                    string onlinePath = Path.Combine(OnlineContentDirectory, mediaFile.filename_disk);
                    yield return SaveMediaToDisk(onlinePath, localPath);
                    yield return StartCoroutine(appState.SetMediaFileTextureFromPath(mediaFile, localPath));
                }

            }
        }
        #endregion

        #region After Populate Content
        private void InvokeDOTweenHelpersStartUp()
        {
            if (sequenceManager != null)
                sequenceManager.CompleteCurrentSequence();

            BaseWindow.onAwakeWindows.Invoke();
            BaseWindow.onStartUpWindows.Invoke();
            BaseStateMachine.onStartUpStateMachines.Invoke();
        }

        /// <summary>
        /// Debug Function to log downloaded profiles that are missing locker locator images.
        /// </summary>
        private void ReportMissingLockerLocatorMedia()
        {
            if (appState != null)
            {
                bool success = true;
                List<LockerProfile> failed = new List<LockerProfile>();
                foreach (LockerProfile profile in appState.data.lockerProfiles)
                {
                    if (!appState.lockerLocatorMedia.ContainsKey(profile.lockerNumber))
                    {
                        success = false;
                        failed.Add(profile);
                    }
                }
                if (success)
                {
                    RLMGLogger.Instance.Log(
                        System.String.Format(
                            "All {0} locker profiles have their locker locator images",
                            appState.data.lockerProfiles.Count
                        ),
                        MESSAGETYPE.INFO
                    );
                }
                else
                {
                    RLMGLogger.Instance.Log(
                        System.String.Format(
                            "The following {0} locker profiles are missing their locker locator images:\n{1}",
                            failed.Count,
                            System.String.Join("\n", failed.Select(p => System.String.Format("{0}: #{1}", p.fullName, p.lockerNumber)))
                        ),
                        MESSAGETYPE.ERROR
                    );
                }
            }
        }
        #endregion
    }
}


