using UnityEngine;
using Newtonsoft.Json;
using System;

namespace JoshKery.USGA.Directus
{
    [Serializable]
    public class MediaFile
    {
        #region Graph Properties
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
        public Texture2D texture;
        public string path_download;
        #endregion


    }
}
