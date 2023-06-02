using System.Collections;
using UnityEngine.Networking;

namespace JoshKery.GenericUI.ContentLoading
{
    public class QueryLoader : ContentLoader
    {
        public string query;
        public string url;
        public string authToken;
        protected override IEnumerator LoadLocalContentSuccess(string text)
        {
            query = text;
            UnityEngine.Debug.Log(text);
            yield return null;
        }

        protected override IEnumerator LoadLocalContentFinish(UnityWebRequest.Result result)
        {
            yield return null;
        }
    }
}


