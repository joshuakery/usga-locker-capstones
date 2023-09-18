using System.IO;
using System.Collections;
using System.Collections.Generic;
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
            /// Hexadecimal code determining this era's color
            /// </summary>
            [JsonProperty("hex")]
            public string hex { get; set; } = "#d13322";

            /// <summary>
            /// Duration in seconds from last user input to return to attract state
            /// </summary>
            [JsonProperty("timeout")]
            public int timeout { get; set; } = 120;

            /// <summary>
            /// Name of directory from which attract media will be loaded.
            /// Paths will be constructed dynamically.
            /// </summary>
            [JsonProperty("attractMediaDirectoryName")]
            public string attractMediaDirectoryName { get; set; } = "attractMedia";

            /// <summary>
            /// Interval between calls for Attract to rotate images.
            /// </summary>
            [JsonProperty("attractInterval")]
            public int attractInterval { get; set; } = 10;

            /// <summary>
            /// Name of directory from which profile backgrounds media will be loaded.
            /// Paths will be constructed dynamically.
            /// </summary>
            [JsonProperty("profileBackgroundsDirectoryName")]
            public string profileBackgroundsDirectoryName { get; set; } = "profileBackgrounds";

            /// <summary>
            /// Name of directory from which locker finder images will be loaded.
            /// Paths will be constructed dynamically.
            /// </summary>
            [JsonProperty("lockerLocatorMediaDirectoryName")]
            public string lockerLocatorMediaDirectoryName { get; set; } = "lockerLocatorMedia";
        }

        #region FIELDS

        /// <summary>
        /// Formulates and submits graphQL query and populates content
        /// </summary>
        private LockerCapstonesContentLoader contentLoader;

        /// <summary>
        /// Controls color palette for UI
        /// </summary>
        [SerializeField]
        private FlexibleUIData flexUIData;

        //todo timeout controller

        [SerializeField]
        private AppState appState;

        [SerializeField]
        private AttractSlideManager attractSlideManager;

        [SerializeField]
        private CarouselAutoSpin autoSpin;

        [SerializeField]
        private ProfileBackgroundManager profileBackgroundManager;

        #endregion

        #region Monobehaviour Methods
        protected override void Awake()
        {
            if (contentLoader == null)
                contentLoader = FindObjectOfType<LockerCapstonesContentLoader>();

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
            SaveContentFileToDisk(text);

            yield return StartCoroutine( PopulateContent(text) );

            onPopulateContentFinish.Invoke();
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

            LockerCapstonesConfigData data = JsonConvert.DeserializeObject<LockerCapstonesConfigData>(text);

            if (contentLoader != null)
            {
                contentLoader.erasByIdIdVariable = data.eraID;

                contentLoader.LoadContent();
            }

            if (flexUIData != null)
            {
                Color color = flexUIData.contentColor1;
                ColorUtility.TryParseHtmlString(data.hex, out color);
                flexUIData.contentColor1 = color;

                if (FlexibleUIBase.allOnSkinUI != null)
                    FlexibleUIBase.allOnSkinUI.Invoke();
            }

            //todo assign timeout value

            if (attractSlideManager != null)
            {
                yield return StartCoroutine(LoadAttractMedia(data.attractMediaDirectoryName));
                attractSlideManager.InstantiateSlideDisplays();
            }

            if (autoSpin != null)
            {
                autoSpin.spinInterval = data.attractInterval;
            }

            if (profileBackgroundManager != null)
            {
                yield return StartCoroutine(LoadProfileBackgrounds(data.profileBackgroundsDirectoryName));
            }

            yield return StartCoroutine(LoadLockerLocatorMedia(data.lockerLocatorMediaDirectoryName));
        }

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


