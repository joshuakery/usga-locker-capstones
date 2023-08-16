using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using JoshKery.USGA.Directus;

namespace JoshKery.USGA.LockerCapstones
{
    public class LockerCapstonesDataWrapper
    {
        [JsonIgnore]
        private LockerCapstonesData _data;

        [JsonProperty("data")]
        public LockerCapstonesData data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;

                if (_data?.accomplishmentTypes != null)
                    _data.accomplishmentsDict = _data.accomplishmentTypes.ToDictionary(a => a.id);

                if (_data?.contentTrails != null)
                    _data.contentTrailsDict = _data.contentTrails.ToDictionary(c => c.id);

                if (_data?.lockerProfiles != null)
                    _data.lockerProfilesDict = _data.lockerProfiles.ToDictionary(p => p.id);
            }
        }
    }

    public class LockerCapstonesData
    {
        [JsonIgnore]
        private Era _era;

        [JsonProperty("eras_by_id")]
        public Era era
        {
            get
            {
                return _era;
            }
            set
            {
                _era = value;

                if (_era != null)
                    _era.BuildContentTrailIDsInThisEraList();
            }
        }

        [JsonProperty("accomplishmentTypes")]
        public List<Accomplishment> accomplishmentTypes { get; set; }

        [JsonIgnore]
        private Dictionary<int, Accomplishment> _accomplishmentsDict;

        [JsonIgnore]
        public Dictionary<int, Accomplishment> accomplishmentsDict
        {
            get
            {
                if (_accomplishmentsDict == null)
                    _accomplishmentsDict = new Dictionary<int, Accomplishment>();

                return _accomplishmentsDict;
            }
            set
            {
                _accomplishmentsDict = value;
            }
        }

        [JsonProperty("contentTrails")]
        public List<ContentTrail> contentTrails { get; set; }

        [JsonIgnore]
        private Dictionary<int, ContentTrail> _contentTrailsDict;

        [JsonIgnore]
        public Dictionary<int, ContentTrail> contentTrailsDict
        {
            get
            {
                if (_contentTrailsDict == null)
                    _contentTrailsDict = new Dictionary<int, ContentTrail>();

                return _contentTrailsDict;
            }
            set
            {
                _contentTrailsDict = value;
            }
        }

        [JsonIgnore]
        public List<LockerProfile> lockerProfiles
        {
            get
            {
                if (era != null)
                    return era.profiles;
                else
                    return null;
            }
        }

        [JsonIgnore]
        private Dictionary<int, LockerProfile> _lockerProfilesDict;

        [JsonIgnore]
        public Dictionary<int, LockerProfile> lockerProfilesDict
        {
            get
            {
                if (_lockerProfilesDict == null)
                    _lockerProfilesDict = new Dictionary<int, LockerProfile>();

                return _lockerProfilesDict;
            }
            set
            {
                _lockerProfilesDict = value;
            }
        }
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

        [JsonIgnore]
        public string fullName
        {
            get
            {
                return firstName + " " + lastName;
            }
        }

        [JsonProperty("birth")]
        public DateTime? birth { get; set; }

        [JsonProperty("birthplace")]
        public string birthplace { get; set; }

        [JsonProperty("death")]
        public DateTime? death { get; set; }

        [JsonIgnore]
        public string lifeDates
        {
            get
            {
                if (birth == null)
                {
                    if (death == null)
                        return "";
                    else
                        return "d. " + death.Value.ToString("MMMM dd, yyyy");
                }
                else
                {
                    if (death == null)
                        return "b. " + birth.Value.ToString("MMMM dd, yyyy");
                    else
                        return death.Value.ToString("MMMM dd, yyyy") + " - " + birth.Value.ToString("MMMM dd, yyyy");
                }
            }
        }

        [JsonProperty("inductionYear")]
        public int inductionYear { get; set; }

        [JsonProperty("lockerNumber")]
        public int lockerNumber { get; set; }

        [JsonProperty("media")]
        public List<MediaItem> media { get; set; }

        [JsonIgnore]
        public bool hasMedia
        {
            get
            {
                return (
                    media != null &&
                    media.Where(item => item.mediaFile != null).ToList().Count > 0
                );
            }
        }

        [JsonProperty("featuredImage")]
        public MediaFile featuredImage { get; set; }

        [JsonProperty("backgroundImage")]
        public MediaFile backgroundImage { get; set; }

        [JsonProperty("accomplishments")]
        public List<EarnedAccomplishmentItem> earnedAccomplishmentItems { get; set; }

        [JsonIgnore]
        public bool hasAccomplishments
        {
            get
            {
                return (
                    earnedAccomplishmentItems != null &&
                    earnedAccomplishmentItems.Where(item => item.earnedAccomplishment != null).ToList().Count > 0
                );
            }
        }

        [JsonProperty("bioImages")]
        public List<MediaItem> bioImages { get; set; }

        [JsonProperty("bioText")]
        public string bioText { get; set; }

        [JsonProperty("quote")]
        public string quote { get; set; }

        [JsonProperty("quoteByline")]
        public string quoteByline { get; set; }

        [JsonProperty("signatureImage")]
        public MediaFile signatureImage { get; set; }

        [JsonProperty("contentTrails")]
        public List<ContentTrailItem> contentTrailItems { get; set; }

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

        [JsonIgnore]
        public string name
        {
            get
            {
                return type.name;
            }
        }

        [JsonIgnore]
        public string description
        {
            get
            {
                if (!System.String.IsNullOrEmpty(customDescription))
                    return customDescription;
                else
                    return type.description;
            }
        }

        [JsonIgnore]
        public MediaFile image
        {
            get
            {
                if (customImage != null)
                    return customImage;
                else
                    return type.image;
            }
        }

        [JsonIgnore]
        public bool hasInfo
        {
            get
            {
                return !System.String.IsNullOrEmpty(description);
            }
        }
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

        [JsonProperty("description")]
        public string description { get; set; }
    }

    [Serializable]
    public class Era
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("startYear")]
        public string startYear { get; set; }

        [JsonProperty("endYear")]
        public string endYear { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("historySlides")]
        public List<HistorySlide> historySlides;

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("profiles")]
        public List<LockerProfile> profiles { get; set; }

        [JsonIgnore]
        private List<int> _contentTrailIDsInThisEra;

        /// <summary>
        /// List of ids actually available for filtering in this era
        /// Prepared in advance for use by FilterButtonsManager.SetContent()
        /// </summary>
        [JsonIgnore]
        public List<int> contentTrailIDs
        {
            get
            {
                if (_contentTrailIDsInThisEra == null)
                    _contentTrailIDsInThisEra = new List<int>();

                return _contentTrailIDsInThisEra;
            }
            set
            {
                _contentTrailIDsInThisEra = value;
            }
        }

        public void BuildContentTrailIDsInThisEraList()
        {
            contentTrailIDs.Clear();

            if (profiles != null)
            {
                foreach (LockerProfile lockerProfile in profiles)
                {
                    if (lockerProfile?.contentTrailItems != null)
                    {
                        foreach (ContentTrailItem item in lockerProfile.contentTrailItems)
                        {
                            if (item?.contentTrail != null)
                            {
                                if (!contentTrailIDs.Contains(item.contentTrail.id))
                                {
                                    contentTrailIDs.Add(item.contentTrail.id);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [Serializable]
    public class HistorySlide
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("image")]
        public MediaFile image { get; set; }

        [JsonProperty("order")]
        public int order { get; set; }

        [JsonProperty("text")]
        public string text { get; set; }

        [JsonProperty("imageCaption")]
        public string imageCaption { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("backgroundVideo")]
        public MediaFile backgroundVideo { get; set; }
    }



}
