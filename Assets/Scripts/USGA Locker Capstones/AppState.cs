using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.ContentLoading;
using JoshKery.USGA.Directus;

namespace JoshKery.USGA.LockerCapstones
{
    [CreateAssetMenu(fileName = "AppState", menuName = "App State")]
    public class AppState : ScriptableObject
    {
        #region FIELDS
        [Header("Data")]
        private LockerCapstonesData _data;
        public LockerCapstonesData data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;

                BuildLockerProfilesDict();
                BuildAccomplishmentsDict();
                BuildContentTrailIDsInThisEraList();
            }
        }

        private Dictionary<int, LockerProfile> _lockerProfilesDict;

        public Dictionary<int, LockerProfile> lockerProfilesDict
        {
            get
            {
                if (_lockerProfilesDict == null)
                    _lockerProfilesDict = new Dictionary<int, LockerProfile>();

                return _lockerProfilesDict;
            }
        }

        private Dictionary<int, Accomplishment> _accomplishmentsDict;
        public Dictionary<int, Accomplishment> accomplishmentsDict
        {
            get
            {
                if (_accomplishmentsDict == null)
                    _accomplishmentsDict = new Dictionary<int, Accomplishment>();

                return _accomplishmentsDict;
            }
        }

        /// <summary>
        /// List of ids actually available for filtering in this era
        /// Prepared in advance for use by FilterButtonsManager.SetContent()
        /// </summary>
        private List<int> _contentTrailIDsInThisEra;
        public List<int> contentTrailIDsInThisEra
        {
            get
            {
                if (_contentTrailIDsInThisEra == null)
                    _contentTrailIDsInThisEra = new List<int>();

                return _contentTrailIDsInThisEra;
            }
        }

        #endregion

        #region Dictionaries
        private void BuildLockerProfilesDict()
        {
            lockerProfilesDict.Clear();

            if (data?.lockerProfiles != null)
            {
                foreach (LockerProfile lockerProfile in data.lockerProfiles)
                {
                    if (lockerProfile != null)
                    {
                        lockerProfilesDict[lockerProfile.id] = lockerProfile;
                    }
                }
            }
        }
        private void BuildAccomplishmentsDict()
        {
            accomplishmentsDict.Clear();

            if (data?.accomplishmentTypes != null)
            {
                foreach (Accomplishment accomplishment in data.accomplishmentTypes)
                {
                    if (accomplishment != null)
                    {
                        accomplishmentsDict[accomplishment.id] = accomplishment;
                    }
                }
            }
        }
        #endregion

        #region Lists
        private void BuildContentTrailIDsInThisEraList()
        {
            contentTrailIDsInThisEra.Clear();

            if (data?.lockerProfiles != null)
            {
                foreach (LockerProfile lockerProfile in data.lockerProfiles)
                {
                    if (lockerProfile?.contentTrailItems != null)
                    {
                        foreach (ContentTrailItem item in lockerProfile.contentTrailItems)
                        {
                            if (item?.contentTrail != null)
                            {
                                if (!contentTrailIDsInThisEra.Contains(item.contentTrail.id))
                                {
                                    contentTrailIDsInThisEra.Add(item.contentTrail.id);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Media File Handling
        public IEnumerator SetMediaFileTextureFromPath(MediaFile mediaFile, string path)
        {
            if (!System.String.IsNullOrEmpty(path))
            {
                yield return MediaLoadingUtility.LoadTexture2DFromPath(path, (Texture2D tex) =>
                {
                    if (mediaFile.texture != null)
                    {
                        Destroy(mediaFile.texture);
                        mediaFile.texture = null;
                    }

                    mediaFile.texture = tex;
                });
            }
        }

        #endregion
    }
}


