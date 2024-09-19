using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using JoshKery.GenericUI.ContentLoading;

namespace JoshKery.USGA.Directus
{
    [Serializable]
    public class MediaFile
    {
        #region Graph Properties
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("filename_download")]
        public string filename_download { get; set; }

        [JsonProperty("filename_disk")]
        public string filename_disk { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }
        #endregion

        #region Other Fields
        [JsonIgnore]
        public Texture2D texture;

        [JsonIgnore]
        public string path_download;

        [JsonIgnore]
        public string local_path;
        #endregion

        public bool HasImageExtension()
        {
            return ContentLoader.FileNameHasImageExtension(filename_disk);
        }

        public bool HasImage()
        {
            return HasImageExtension() && texture != null;
        }

        public bool HasVideoExtension()
        {
            return ContentLoader.FileNameHasVideoExtension(filename_disk);
        }
    }
}
