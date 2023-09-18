using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using JoshKery.GenericUI.ContentLoading;
using JoshKery.USGA.Directus;

namespace JoshKery.USGA.LockerCapstones
{
    [CreateAssetMenu(fileName = "AppState", menuName = "App State")]
    public class AppState : ScriptableObject
    {
        #region FIELDS
        [Header("Attract")]
        private List<Texture2D> _attractMedia;
        public List<Texture2D> attractMedia
        {
            get
            {
                if (_attractMedia == null)
                    _attractMedia = new List<Texture2D>();

                return _attractMedia;
            }
        }

        [Header("Profile Backgrounds")]
        private List<Texture2D> _profileBackgrounds;
        public List<Texture2D> profileBackgrounds
        {
            get
            {
                if (_profileBackgrounds == null)
                    _profileBackgrounds = new List<Texture2D>();

                return _profileBackgrounds;
            }
        }

        [Header("Locker Finder Media")]
        private Dictionary<int, Texture2D> _lockerLocatorMedia;

        /// <summary>
        /// Maps locker numbers to "Locker Finder" images for each locker.
        /// </summary>
        public Dictionary<int, Texture2D> lockerLocatorMedia
        {
            get
            {
                if (_lockerLocatorMedia == null)
                    _lockerLocatorMedia = new Dictionary<int, Texture2D>();

                return _lockerLocatorMedia;
            }
        }

        [Header("Data")]
        public LockerCapstonesData data;
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


