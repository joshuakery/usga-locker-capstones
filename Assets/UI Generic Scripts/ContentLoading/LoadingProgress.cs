namespace JoshKery.GenericUI.ContentLoading
{
    public class LoadingProgress
    {
        public string mainMessage;
        public string detailMessage;

        public float loaded;
        public float totalToLoad;

        public float progress
        {
            get
            {
                if (totalToLoad != 0)
                    return loaded / totalToLoad;
                else
                    return -1;
            }
        }

        public LoadingProgress(string m, string d, float total)
        {
            mainMessage = m;
            detailMessage = d;
            loaded = 0;
            totalToLoad = total;
        }

        public void ResetProgress(string m, string d, float total)
        {
            mainMessage = m;
            detailMessage = d;
            loaded = 0;
            totalToLoad = total;
        }
    }
}

