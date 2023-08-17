using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using JoshKery.GenericUI.Events;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileModulesManager : LockerCapstonesWindow
    {
        private static IntEvent _onResetContent;

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

                    if (profileBiographyFieldsManager != null)
                    {
                        profileBiographyFieldsManager.gameObject.SetActive(true);
                        profileBiographyFieldsManager.SetContent(lockerProfile);
                    }
                        

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
                }  
            }

        }

    }
}


