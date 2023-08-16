using UnityEngine;
using Newtonsoft.Json;
using System;

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
        #endregion


    }
}
