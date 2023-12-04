using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.AspectRatio;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class LockerLocatorDrawer : LockerCapstonesWindow
    {
        [SerializeField]
        RawImageManager riManager;

        private LockerProfile currentProfile;

        /// <summary>
        /// Enabling this will be used to close the drawer when the user clicks outside it
        /// </summary>
        [SerializeField]
        private ClickNotOnRTHelper clickNotOnRTHelper;

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

        protected override DG.Tweening.Sequence _Open(SequenceType sequenceType = SequenceType.UnSequenced, float atPosition = 0)
        {
            
            if (clickNotOnRTHelper != null)
                clickNotOnRTHelper.doCheck = true;

            return base._Open(sequenceType, atPosition);
        }

        protected override DG.Tweening.Sequence _Close(SequenceType sequenceType = SequenceType.UnSequenced, float atPosition = 0)
        {
            if (clickNotOnRTHelper != null)
                clickNotOnRTHelper.doCheck = false;

            return base._Close(sequenceType, atPosition);
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


