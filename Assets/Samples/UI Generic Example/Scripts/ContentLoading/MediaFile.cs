using UnityEngine;

namespace JoshKery.GenericUI.Example
{
    public class MediaFile
    {
        public string path;
        public string caption;
        public Texture2D texture;

        public MediaFile(string p, string c, Texture2D t)
        {
            path = p;
            caption = c;
            texture = t;
        }
    }
}
