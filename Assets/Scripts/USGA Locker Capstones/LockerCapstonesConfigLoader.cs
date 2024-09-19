using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JoshKery.GenericUI.ContentLoading;
using JoshKery.GenericUI.Carousel;
using UnityEngine.Networking;
using Newtonsoft.Json;
using rlmg.logging;

namespace JoshKery.USGA.LockerCapstones
{
    public class LockerCapstonesConfigLoader : ContentLoader
    {
        public class LockerCapstonesConfigData
        {
            /// <summary>
            /// Era ID to use as variable in graphQL query of LockerCapstonesContentLoader
            /// </summary>
            [JsonProperty("eraID")]
            public int eraID { get; set; } = 1;
            
            /// <summary>
            /// How long it will take for the Choose Era prompt on the Setup Window to dismiss and proceed with chosen era
            /// </summary>
            [JsonProperty("chooseEraTimeout")]
            public int chooseEraTimeout { get; set; } = 30;

            /// <summary>
            /// Duration in seconds from last user input to return to attract state
            /// </summary>
            [JsonProperty("timeout")]
            public int timeout { get; set; } = 120;

            /// <summary>
            /// Interval between calls for Attract to rotate images.
            /// </summary>
            [JsonProperty("attractInterval")]
            public int attractInterval { get; set; } = 10;

            /// <summary>
            /// Name of directory from which locker finder images will be loaded.
            /// Paths will be constructed dynamically.
            /// </summary>
            [JsonProperty("lockerLocatorMediaDirectoryName")]
            public string lockerLocatorMediaDirectoryName { get; set; } = "lockerLocatorMedia";

            /// <summary>
            /// If true, will attempt to query Directus CMS. Otherwise will default to already loaded data.
            /// </summary>
            [JsonProperty("doLoadFromCMS")]
            public bool doLoadFromCMS { get; set; } = true;

            /// <summary>
            /// Server hosting CMS
            /// </summary>
            [JsonProperty("apiServer")]
            public string apiServer { get; set; }

            /// <summary>
            /// Auth token for CMS
            /// </summary>
            [JsonProperty("apiToken")]
            public string apiToken { get; set; }

            /// <summary>
            /// GraphQL endpoint for CMS
            /// </summary>
            [JsonProperty("apiEndpoint")]
            public string apiEndpoint { get; set; }

            [JsonProperty("assetsEndpoint")]
            public string assetsEndpoint { get; set; }

            /// <summary>
            /// List of possible eras to query for across the different instances of Locker Capstones
            /// </summary>
            [JsonProperty("eraOptions")]
            public EraOption[] eraOptions { get; set; }
        }

        public class EraOption
        {
            /// <summary>
            /// Display name in the setup window; does not need to correspond to any CMS data
            /// </summary>
            [JsonProperty("name")]
            public string name { get; set; }

            /// <summary>
            /// Era ID to use as variable in graphQL query of LockerCapstonesContentLoader
            /// </summary>
            [JsonProperty("eraID")]
            public int eraID { get; set; } = 1;

            /// <summary>
            /// Hexadecimal code determining this era's color
            /// </summary>
            [JsonProperty("hex")]
            public string hex { get; set; } = "#1D3C34";

            /// <summary>
            /// Name of directory from which attract media will be loaded.
            /// Paths will be constructed dynamically.
            /// </summary>
            [JsonProperty("attractMediaDirectoryName")]
            public string attractMediaDirectoryName { get; set; } = "attractMedia/theGreatGolfBoom";

            /// <summary>
            /// Name of directory from which profile backgrounds media will be loaded.
            /// Paths will be constructed dynamically.
            /// </summary>
            [JsonProperty("profileBackgroundsDirectoryName")]
            public string profileBackgroundsDirectoryName { get; set; } = "profileBackgrounds/theGreatGolfBoom";
        }

        #region FIELDS

        private LockerCapstonesConfigData data;

        /// <summary>
        /// Formulates and submits graphQL query and populates content
        /// </summary>
        private LockerCapstonesContentLoader contentLoader;

        /// <summary>
        /// Controls color palette for UI
        /// </summary>
        [SerializeField]
        private FlexibleUIData flexUIData;

        [SerializeField]
        private AppState appState;

        [SerializeField]
        private LockerCapstonesAttractTimeout attractTimeout;

        [SerializeField]
        private AttractSlideManager attractSlideManager;

        [SerializeField]
        private CarouselAutoSpin autoSpin;

        [SerializeField]
        private ProfileBackgroundManager profileBackgroundManager;

        public delegate void OnEraOptionsLoadedEvent(EraOption[] options, int selectedIndex);
        public OnEraOptionsLoadedEvent onEraOptionsLoaded;

        /// <summary>
        /// 
        /// </summary>
        private bool doProceedToLoadContent = false;

        public TimeSpan timeRemainingToProceed;

        public delegate void OnTimeUpdatedEvent();
        public OnTimeUpdatedEvent onTimeUpdated;

        #endregion

        #region Monobehaviour Methods
        protected override void Awake()
        {
            if (contentLoader == null)
                contentLoader = FindObjectOfType<LockerCapstonesContentLoader>();

            doProceedToLoadContent = false;
            timeRemainingToProceed = TimeSpan.FromSeconds(0);

            base.Awake();
        }
        #endregion

        #region Load Local Content Overrides
        protected override IEnumerator LoadLocalContentFinish(UnityWebRequest.Result result)
        {
            yield return null;
        }

        protected override IEnumerator LoadLocalContentSuccess(string text)
        {
            yield return StartCoroutine( PopulateContent(text) );
        }
        #endregion

        #region Save Local Content to Disk Override
        public override void SaveContentFileToDisk(string text)
        {
            string aux = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<LockerCapstonesConfigData>(text), Formatting.Indented);

            base.SaveContentFileToDisk(aux);
        }
        #endregion

        #region PopulateContent
        private IEnumerator PopulateContent(string text)
        {
            yield return null;

            data = JsonConvert.DeserializeObject<LockerCapstonesConfigData>(text);
            Dictionary<int, EraOption> eras = data.eraOptions.ToDictionary(e => e.eraID);

            onEraOptionsLoaded?.Invoke(data.eraOptions, data.eraOptions.ToList().FindIndex(e => e.eraID == data.eraID));
            yield return StartCoroutine(WaitForEraChoice(data.chooseEraTimeout));

            //Save afterward so that a selection of eraID is reflected in the config
            SaveContentFileToDisk(JsonConvert.SerializeObject(data));

            if (contentLoader != null)
            {
                contentLoader.graphQLURL = data.apiServer + "/" + data.apiEndpoint;
                contentLoader.authToken = data.apiToken;
                contentLoader.OnlineContentDirectory = data.apiServer + "/" + data.assetsEndpoint;

                contentLoader.erasByIdIdVariable = data.eraID;
                contentLoader.doDefaultLocalLoadContent = !data.doLoadFromCMS;

                // The other media will load in the background to the larger content media loading
                contentLoader.LoadContent();
            }

            if (flexUIData != null)
            {
                Color color = flexUIData.contentColor1;
                ColorUtility.TryParseHtmlString(eras[data.eraID].hex, out color);
                flexUIData.contentColor1 = color;

                if (FlexibleUIBase.allOnSkinUI != null)
                    FlexibleUIBase.allOnSkinUI.Invoke();
            }

            if (attractTimeout != null)
            {
                attractTimeout.timeoutDuration = data.timeout;
                attractTimeout.ResetTimer();
            }

            if (attractSlideManager != null)
            {
                yield return StartCoroutine(LoadAttractMedia(eras[data.eraID].attractMediaDirectoryName));
                attractSlideManager.InstantiateSlideDisplays();
            }

            if (autoSpin != null)
            {
                autoSpin.spinInterval = data.attractInterval;
            }

            if (profileBackgroundManager != null)
            {
                yield return StartCoroutine(LoadProfileBackgrounds(eras[data.eraID].profileBackgroundsDirectoryName));
                profileBackgroundManager.SetRandom();
            }

            yield return StartCoroutine(LoadLockerLocatorMedia(data.lockerLocatorMediaDirectoryName));

            yield return null;

            onPopulateContentFinish?.Invoke();
        }

        #region WaitForEraChoice
        private IEnumerator WaitForEraChoice(int duration)
        {
            DateTime timeout = DateTime.Now + TimeSpan.FromSeconds(duration);

            while (!doProceedToLoadContent)
            {
                yield return null;

                if (DateTime.Now > timeout)
                {
                    doProceedToLoadContent = true;
                    timeRemainingToProceed = TimeSpan.FromSeconds(0);
                    onTimeUpdated?.Invoke();
                }
                else
                {
                    timeRemainingToProceed = timeout - DateTime.Now;
                    onTimeUpdated?.Invoke();
                }    
            }

            timeRemainingToProceed = TimeSpan.FromSeconds(0);
            onTimeUpdated?.Invoke();
        }

        public void ProceedToLoadContent()
        {
            doProceedToLoadContent = true;
        }

        public void SelectEra(int index)
        {
            if (data == null) { return; }

            EraOption selectedOption = data.eraOptions[index];

            data.eraID = selectedOption.eraID;
        }
        #endregion

        #region Populate Content Helpers
        private IEnumerator LoadAttractMedia(string attractMediaDirName)
        {
            yield return null;

            if (appState != null)
            {
                appState.attractMedia.Clear();

                string localDirPath = Path.Combine(LocalContentDirectory, attractMediaDirName);

                if (Directory.Exists(localDirPath))
                {
                    string[] fileEntries = Directory.GetFiles(localDirPath);
                    foreach (string path in fileEntries)
                    {
                        if (!System.String.IsNullOrEmpty(path) && MediaLoadingUtility.IsImageFile(path))
                        {
                            yield return MediaLoadingUtility.LoadTexture2DFromPath(path, (Texture2D tex) =>
                            {
                                appState.attractMedia.Add(tex);
                            });
                        }
                    }

                }
            }
        }

        private IEnumerator LoadProfileBackgrounds(string profileBackgroundsDirName)
        {
            yield return null;

            int count = 0;

            if (appState != null)
            {
                appState.profileBackgrounds.Clear();

                string localDirPath = Path.Combine(LocalContentDirectory, profileBackgroundsDirName);

                if (Directory.Exists(localDirPath))
                {
                    string[] fileEntries = Directory.GetFiles(localDirPath);
                    foreach (string path in fileEntries)
                    {
                        if (!System.String.IsNullOrEmpty(path) && MediaLoadingUtility.IsImageFile(path))
                        {
                            yield return MediaLoadingUtility.LoadTexture2DFromPath(path, (Texture2D tex) =>
                            {
                                appState.profileBackgrounds.Add(tex);
                                count++;
                            });
                        }
                    }

                }
            }

            RLMGLogger.Instance.Log(
                System.String.Format(
                    "{0} images loaded for backgrounds behind profile cards.",
                    count
                ),
                MESSAGETYPE.INFO
            );
        }

        private IEnumerator LoadLockerLocatorMedia(string lockerLocatorMediaDirName)
        {
            yield return null;

            if (appState != null)
            {
                appState.lockerLocatorMedia.Clear();

                string localDirPath = Path.Combine(LocalContentDirectory, lockerLocatorMediaDirName);

                if (Directory.Exists(localDirPath))
                {
                    string[] fileEntries = Directory.GetFiles(localDirPath);
                    foreach (string path in fileEntries)
                    {
                        if (!System.String.IsNullOrEmpty(path) && MediaLoadingUtility.IsImageFile(path))
                        {
                            yield return MediaLoadingUtility.LoadTexture2DFromPath(path, (Texture2D tex) =>
                            {
                                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);

                                int number;

                                bool success = int.TryParse(fileNameWithoutExtension, out number);
                                if (success)
                                {
                                    appState.lockerLocatorMedia[number] = tex;
                                }
                                else
                                {
                                    RLMGLogger.Instance.Log(
                                        System.String.Format(
                                            "Failed to parse a locker number from the following file name: {0}.\nPlease make the filename only digits that correspond to a profile's locker number in the content management system.",
                                            path
                                        ),
                                        MESSAGETYPE.ERROR
                                    );
                                }
                            });
                        }
                    }
                }
            }
        }

        
        #endregion
        #endregion
    }
}


