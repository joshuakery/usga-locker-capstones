using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.AspectRatio;

namespace JoshKery.USGA.LockerCapstones
{
    public class LockerLocatorDrawer : LockerCapstonesWindow
    {
        [SerializeField]
        RawImageManager riManager;

        private LockerProfile currentProfile;

        protected override void OnEnable()
        {
            base.OnEnable();

            MainCanvasStateMachine.beforeAnimateToProfile += CloseAndComplete;

            ProfileModulesManager.onResetContent.AddListener(SetContent);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            MainCanvasStateMachine.beforeAnimateToProfile -= CloseAndComplete;

            ProfileModulesManager.onResetContent.RemoveListener(SetContent);
        }

        private void CloseAndComplete()
        {
            Close(GenericUI.DOTweenHelpers.SequenceType.CompleteImmediately);
        }

        public void SetContent(int profileID)
        {
            if (appState != null && riManager != null)
            {
                if (appState.data.lockerProfilesDict.ContainsKey(profileID))
                {
                    currentProfile = appState.data.lockerProfilesDict[profileID];

                    if (currentProfile != null)
                    {
                        if (appState.lockerLocatorMedia.ContainsKey(currentProfile.lockerNumber))
                        {
                            riManager.texture = appState.lockerLocatorMedia[currentProfile.lockerNumber];
                        }
                        else
                        {
                            riManager.texture = null;
                        }
                    }                  
                }
            }
        }
    }
}


