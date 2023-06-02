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


