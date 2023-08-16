using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.ContentLoading;
using JoshKery.GenericUI.Carousel;
using UnityEngine.Networking;
using Newtonsoft.Json;

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

            [JsonProperty("attractInterval")]
            public int attractInterval { get; set; } = 10;
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
        #endregion
        #endregion
    }
}


