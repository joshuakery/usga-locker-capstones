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
            onLoadingProgress?.Invoke("Starting to Load Content via GraphQL");
            yield return LoadGraphContent().AsIEnumerator();
        }

        private async Task LoadGraphContent()
        {
            object variables = new { erasByIdId = erasByIdIdVariable };

            UnityWebRequest request = await HttpHandler.PostAsync(graphQLURL, query, variables, operationName, authToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                await GraphResponseFail(request);
            }
            else
            {
                await GraphResponseSuccess(request.downloadHandler.text);
            }
        }

        private async Task GraphResponseSuccess(string text)
        {
            int n = 50;
            string message = string.Format("Graph response success! First {0} chars of response: {1}", n, text.Substring(0, n));

            onLoadingDetails?.Invoke(message);

            RLMGLogger.Instance.Log(message,MESSAGETYPE.INFO);

            SaveContentFileToDisk(text);

            await PopulateContent(text);

            await new WaitForEndOfFrame(); //TODO why do I do this?
        }

        private async Task GraphResponseFail(UnityWebRequest request)
        {
            string message = string.Format("Graph response error! Error message: {0}", request.error);

            onLoadingError?.Invoke(message);

            RLMGLogger.Instance.Log(message,MESSAGETYPE.ERROR);

            //TODO UI display of error handling and option to try again

            RLMGLogger.Instance.Log(
                "Falling back to locally saved content...",
                MESSAGETYPE.INFO
            );

            await LoadLocalContent();
        }
        #endregion

        #region Load Local Content Fallback
        protected override IEnumerator LoadLocalContentSuccess(string text)
        {
            int n = 50;
            string message = string.Format("Local content loaded successively! First {0} chars of response: {1}", n, text.Substring(0, n));

            onLoadingDetails?.Invoke(message);

            RLMGLogger.Instance.Log(message, MESSAGETYPE.INFO);

            yield return StartCoroutine(PopulateContent(text));

            SaveContentFileToDisk(text);

            yield return new WaitForEndOfFrame();
        }

        protected override IEnumerator LoadLocalContentFinish(UnityWebRequest.Result result)
        {
            yield return null;
        }
        #endregion

        #region Save Local Content to Disk Override
        /// <summary>
        /// Prettifies the json before base.SaveContentFileToDisk
        /// </summary>
        /// <param name="text"></param>
        public override void SaveContentFileToDisk(string text)
        {
            string aux = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<LockerCapstonesDataWrapper>(text), Formatting.Indented);

            base.SaveContentFileToDisk(aux);
        }
        #endregion



        #region PopulateContent
        private IEnumerator PopulateContent(string text)
        {
            onLoadingProgress?.Invoke("Populating Content", "Starting to populate content...");

            yield return null;
            //TODO manage displays for progess updates?

            LockerCapstonesDataWrapper wrapper = JsonConvert.DeserializeObject<LockerCapstonesDataWrapper>(text);
            appState.data = wrapper.data;

            yield return StartCoroutine(LoadLockerCapstonesMedia());

            yield return null;

            LockerCapstonesWindow.onSetContent.Invoke();
            LockerCapstonesStateMachine.onSetContent.Invoke();
            FilterButtonsManager.onSetContent.Invoke();

            yield return null;

            onPopulateContentFinish?.Invoke();

            onLoadingDetails?.Invoke("Finished populating content.");
        }

        private IEnumerator LoadLockerCapstonesMedia()
        {
            if (appState?.data == null) { yield break; }

            // AccomplishmentIcons must come first in order to assign images to the locker profiles' accomplishments
            if (appState.data.accomplishmentIcons != null)
            {
                onLoadingProgress?.Invoke("Loading Achivements", "Starting to load achievement icons...", appState.data.accomplishmentIcons.Count, 0);

                for (int i=0; i< appState.data.accomplishmentIcons.Count; i++)
                {
                    AccomplishmentIcon icon = appState.data.accomplishmentIcons[i];

                    yield return StartCoroutine(LoadMediaFromMediaFile(icon?.image));

                    //Report a more detailed error statement if the media did not load
                    if (icon.image.texture == null)
                        RLMGLogger.Instance.Log(
                            string.Format(
                                "Failed to load media for achievement icon {0}, for image titled {1}",
                                icon.id,
                                icon.image.title
                               ),
                            MESSAGETYPE.ERROR
                        );

                    onLoadingProgress?.Invoke(
                        "Loading Achivements",
                        "Finished loading achievement icon " + icon.id,
                        appState.data.accomplishmentIcons.Count,
                        i+1
                    );
                }

                onLoadingDetails?.Invoke("Finished loading achievement icons.");
            }

            if (appState.data.lockerProfiles != null)
            {
                onLoadingProgress?.Invoke("Loading Locker Profile Media", "Starting to load locker profile media...", appState.data.lockerProfiles.Count, 0);

                for (int i=0; i<appState.data.lockerProfiles.Count; i++)
                {
                    LockerProfile lockerProfile = appState.data.lockerProfiles[i];

                    yield return StartCoroutine(LoadMediaForLockerProfile(lockerProfile));

                    onLoadingProgress?.Invoke(
                        "Loading Locker Profile Media",
                        "Finished loading locker profile media for " + lockerProfile.fullName,
                        appState.data.lockerProfiles.Count,
                        i + 1
                    );
                }

                onLoadingDetails?.Invoke("Finished loading locker profile media.");
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
                //yield return StartCoroutine(LoadMediaFromMediaFile(lockerProfile.featuredImage, LoadMediaMethod.CustomSize, 432, 432));
                yield return StartCoroutine(LoadMediaFromMediaFile(lockerProfile.featuredImage));

                if (lockerProfile.media != null)
                {
                    foreach (MediaItem item in lockerProfile.media)
                    {
                        yield return StartCoroutine(LoadMediaFromMediaFile(item?.mediaFile, LoadMediaMethod.FitToParent, 1630, 1200));

                        //Report a more detailed error statement if the media did not load
                        if (item.mediaFile.texture == null)
                            RLMGLogger.Instance.Log(
                                string.Format(
                                    "Failed to load media for locker profile {0}, for media gallery media titled {1}",
                                    lockerProfile.fullName,
                                    item.mediaFile.title
                                   ),
                                MESSAGETYPE.ERROR
                            );
                    }
                }

                yield return StartCoroutine(LoadMediaFromMediaFile(lockerProfile.signatureImage));

                if (lockerProfile.bioImages != null)
                {
                    foreach (MediaItem item in lockerProfile.bioImages)
                    {
                        yield return StartCoroutine(LoadMediaFromMediaFile(item?.mediaFile));

                        //Report a more detailed error statement if the media did not load
                        if (item.mediaFile.texture == null)
                            RLMGLogger.Instance.Log(
                                string.Format(
                                    "Failed to load media for locker profile {0}, for bio image titled {1}",
                                    lockerProfile.fullName,
                                    item.mediaFile.title
                                   ),
                                MESSAGETYPE.ERROR
                            );
                    }
                }
            }
        }

        private IEnumerator LoadMediaForEra(Era era)
        {
            if (era?.historySlides != null)
            {
                onLoadingProgress?.Invoke("Loading Era History Slides Media", "Starting to load era history slides media...", era.historySlides.Count, 0);

                for (int i=0; i<era.historySlides.Count; i++)
                {
                    HistorySlide slide = era.historySlides[i];

                    yield return StartCoroutine(LoadMediaFromMediaFile(slide?.image));

                    //Report a more detailed error statement if the media did not load
                    if (slide.image.texture == null)
                        RLMGLogger.Instance.Log(
                            string.Format(
                                "Failed to load media for era history slide {0}, for image titled {1}",
                                slide.title,
                                slide.image.title
                               ),
                            MESSAGETYPE.ERROR
                        );

                    yield return StartCoroutine(LoadMediaFromMediaFile(slide?.backgroundVideo));

                    //Report a more detailed error statement if the media did not load
                    if (slide.image.texture == null)
                        RLMGLogger.Instance.Log(
                            string.Format(
                                "Failed to load background media for era history slide {0}, for image titled {1}",
                                slide.title,
                                slide.backgroundVideo.title
                               ),
                            MESSAGETYPE.ERROR
                        );

                    onLoadingProgress?.Invoke(
                        "Loading Era History Slides Media",
                        "Finished loading era history slide \"" + slide.title + "\"",
                        era.historySlides.Count,
                        i + 1
                    );
                }

                onLoadingDetails?.Invoke("Finished loading era history slides media.");
            }
        }

        private enum LoadMediaMethod
        {
            Default = 0,
            CustomSize = 1,
            FitToParent = 2
        }

        private IEnumerator LoadMediaFromMediaFile(MediaFile mediaFile, LoadMediaMethod loadMethod = LoadMediaMethod.Default, int width = 0, int height = 0)
        {
            yield return StartCoroutine(LoadMediaFromMediaFile(mediaFile));

            if (mediaFile.texture != null)
            {
                switch (loadMethod)
                {
                    case LoadMediaMethod.CustomSize:
                        mediaFile.texture = Resize(mediaFile.texture, width, height);
                        break;
                    case LoadMediaMethod.FitToParent:
                        float ratio = (float)mediaFile.texture.width / (float)mediaFile.texture.height;
                        float parentRatio = (float)width / (float)height;
                        if (ratio > parentRatio)
                            height = Mathf.RoundToInt(width * (1f / ratio));
                        else if (ratio < parentRatio)
                            width = Mathf.RoundToInt(height * ratio);
                        mediaFile.texture = Resize(mediaFile.texture, width, height);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static Texture2D Resize(Texture2D source, int newWidth, int newHeight)
        {
            source.filterMode = FilterMode.Trilinear;
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            rt.filterMode = FilterMode.Point;
            RenderTexture.active = rt;
            Graphics.Blit(source, rt);
            Texture2D nTex = new Texture2D(newWidth, newHeight);
            nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
            nTex.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
            Destroy(source);
            return nTex;
        }

        private IEnumerator LoadMediaFromMediaFile(MediaFile mediaFile)
        {
            if (mediaFile != null)
            {
                string localPath = GetLocalMediaPath(mediaFile.filename_disk);

                if (!File.Exists(localPath) && !doDefaultLocalLoadContent)
                {
                    string onlinePath = Path.Combine(OnlineContentDirectory, mediaFile.filename_disk);
                    yield return SaveMediaToDisk(onlinePath, localPath);
                }

                if (File.Exists(localPath))
                {
                    onLoadingDetails?.Invoke("Loading media from local path: " + localPath);
                    yield return StartCoroutine(AppState.SetMediaFileTextureFromPath(mediaFile, localPath));
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


