using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.ContentLoading;

namespace JoshKery.GenericUI.Example
{
    [CreateAssetMenu(fileName = "ExampleMediaState", menuName = "Example Media State")]
    public class ExampleMediaState : ScriptableObject
    {
        [Header("Media")]
        public Dictionary<string, MediaFile> mediaFiles;

        //-----METHODS-----
        public IEnumerator CreateMediaFileObject(string key, string path, string caption = "")
        {
            if (!System.String.IsNullOrEmpty(path))
            {
                yield return MediaLoadingUtility.LoadTexture2DFromPath(path, (Texture2D tex) =>
                {
                    if (mediaFiles == null) { mediaFiles = new Dictionary<string, MediaFile>(); }

                    if (mediaFiles.ContainsKey(key))
                    {
                        MediaFile existingMediaFile = mediaFiles[key];
                        Destroy(existingMediaFile.texture);
                        existingMediaFile.texture = null;
                    }

                    mediaFiles[key] = new MediaFile
                    (
                        path,
                        caption,
                        tex
                    );
                });
            }

        }

        public Texture2D GetMediaTexture(string key)
        {
            Texture2D tex = null;
            if (key != null)
            {
                if (mediaFiles != null && mediaFiles.ContainsKey(key))
                {
                    MediaFile mediaFile = mediaFiles[key];
                    tex = mediaFile?.texture;
                }
            }
            return tex;
        }
    }
}


