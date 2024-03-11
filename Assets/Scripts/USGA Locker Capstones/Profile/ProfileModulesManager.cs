using UnityEngine;
using JoshKery.GenericUI.Events;


namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileModulesManager : LockerCapstonesWindow
    {
        private static IntEvent _onResetContent;

        /// <summary>
        /// Invoked when animating to profile. Int is id of locker profile to display.
        /// </summary>
        public static IntEvent onResetContent
        {
            get
            {
                if (_onResetContent == null)
                    _onResetContent = new IntEvent();

                return _onResetContent;
            }
        }

        [SerializeField]
        private ProfileHeaderFieldsManager profileHeaderFieldsManager;

        [SerializeField]
        private ModulePaginatorManager modulePaginatorManager;

        [SerializeField]
        private BiographyFieldsManager profileBiographyFieldsManager;

        [SerializeField]
        private AccomplishmentsManager profileAccomplishmentsManager;

        [SerializeField]
        private MediaGalleryManager profileMediaGalleryManager;

        protected override void OnEnable()
        {
            base.OnEnable();

            onResetContent.AddListener(SetContent);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onResetContent.RemoveListener(SetContent);
        }

        public void SetContent(int id)
        {
            if (appState?.data == null) { return; }

            if (appState.data.lockerProfilesDict.ContainsKey(id))
            {
                LockerProfile lockerProfile = appState.data.lockerProfilesDict[id];

                if (lockerProfile != null)
                {
                    
                    if (profileHeaderFieldsManager != null)
                        profileHeaderFieldsManager.SetContent(lockerProfile);

                    #region Biography Module
                    if (profileBiographyFieldsManager != null)
                    {
                        profileBiographyFieldsManager.gameObject.SetActive(true);
                        profileBiographyFieldsManager.SetContent(lockerProfile);
                    }
                    #endregion

                    #region Accomplishments Module
                    if (profileAccomplishmentsManager != null)
                    {
                        if (lockerProfile.hasAccomplishments)
                        {
                            profileAccomplishmentsManager.gameObject.SetActive(true);
                            profileAccomplishmentsManager.SetContent(lockerProfile);
                        }
                        else
                        {
                            profileAccomplishmentsManager.gameObject.SetActive(false);
                        }
                    }
                    #endregion

                    #region Media Gallery Module
                    if (profileMediaGalleryManager != null)
                    {
                        if (lockerProfile.hasMedia)
                        {
                            profileMediaGalleryManager.gameObject.SetActive(true);
                            profileMediaGalleryManager.SetContent(lockerProfile);
                        }
                        else
                        {
                            profileMediaGalleryManager.gameObject.SetActive(false);
                        }
                    }
                    #endregion

                    #region Pagination
                    if (modulePaginatorManager != null)
                    {
                        modulePaginatorManager.ClearAllDisplays();

                        //because of vertical scroll snap limitations, we have to instantiate the elements in reverse order
                        //and then reverse their layout in the horizontal layout group so that they appear in bio>achievements>media order
                        if (lockerProfile.hasMedia)
                        {
                            modulePaginatorManager.InstantiatePaginator("Media Gallery");
                        }

                        if (lockerProfile.hasAccomplishments)
                        {
                            modulePaginatorManager.InstantiatePaginator("Achievements");
                        }

                        modulePaginatorManager.InstantiatePaginator("Biography");

                        modulePaginatorManager.ResetScroll();
                    }                        
                    #endregion
                }
            }

        }

    }
}


