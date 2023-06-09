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
        private ProfileBiographyFieldsManager profileBiographyFieldsManager;

        [SerializeField]
        private ProfileAccomplishmentsManager profileAccomplishmentsManager;

        [SerializeField]
        private ProfileMediaGalleryManager profileMediaGalleryManager;

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
            if (appState == null) { return; }

            if (appState.lockerProfilesDict.ContainsKey(id))
            {
                LockerProfile lockerProfile = appState.lockerProfilesDict[id];

                profileHeaderFieldsManager.SetContent(lockerProfile);
                profileBiographyFieldsManager.SetContent(lockerProfile);

                if (profileAccomplishmentsManager != null)
                {
                    if (lockerProfile?.earnedAccomplishmentItems?.Where(item => item.earnedAccomplishment != null).ToList().Count > 0)
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
                    if (lockerProfile?.media?.Where(item => item.mediaFile != null).ToList().Count > 0)
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


