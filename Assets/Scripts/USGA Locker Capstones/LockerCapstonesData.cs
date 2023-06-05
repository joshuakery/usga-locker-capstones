using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using JoshKery.USGA.Directus;

namespace JoshKery.USGA.LockerCapstones
{
    public class LockerCapstonesDataWrapper
    {
        [JsonProperty("data")]
        public LockerCapstonesData data { get; set; }
    }

    public class LockerCapstonesData
    {
        [JsonProperty("lockerProfiles")]
        public List<LockerProfile> lockerProfiles { get; set; }

        [JsonProperty("accomplishmentTypes")]
        public List<Accomplishment> accomplishmentTypes { get; set; }

    }

    [Serializable]
    public class LockerProfile
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("firstName")]
        public string firstName { get; set; }

        [JsonProperty("lastName")]
        public string lastName { get; set; }

        [JsonProperty("inductionYear")]
        public int inductionYear { get; set; }

        [JsonProperty("lockerNumber")]
        public int lockerNumber { get; set; }

        [JsonProperty("media")]
        public List<MediaItem> media { get; set; }

        [JsonProperty("featuredImage")]
        public MediaFile featuredImage { get; set; }

        [JsonProperty("accomplishments")]
        public List<EarnedAccomplishmentItem> earnedAccomplishmentItems { get; set; }

        [JsonProperty("bioImages")]
        public List<MediaItem> bioImages { get; set; }

        [JsonProperty("bioText")]
        public string bioText { get; set; }

        [JsonProperty("quote")]
        public string quote { get; set; }

        [JsonProperty("signatureImage")]
        public MediaFile signatureImage { get; set; }

    }

    [Serializable]
    public class MediaItem
    {
        [JsonProperty("directus_files_id")]
        public MediaFile mediaFile { get; set; }
    }

    [Serializable]
    public class EarnedAccomplishmentItem
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("earnedAccomplishments_id")]
        public EarnedAccomplishment earnedAccomplishment;
    }

    [Serializable]
    public class EarnedAccomplishment
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("timesEarned")]
        public int timesEarned { get; set; }

        [JsonProperty("customImage")]
        public MediaFile customImage { get; set; }

        [JsonProperty("customDescription")]
        public string customDescription { get; set; }

        [JsonProperty("type")]
        public Accomplishment type { get; set; }
    }

    [Serializable]
    public class Accomplishment
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("image")]
        public MediaFile image { get; set; }
    }

    [Serializable]
    public class ContentTrailItem
    {
        [JsonProperty("contentTrails_id")]
        public ContentTrail contentTrail { get; set; }
    }

    [Serializable]
    public class ContentTrail
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }
    }



}
