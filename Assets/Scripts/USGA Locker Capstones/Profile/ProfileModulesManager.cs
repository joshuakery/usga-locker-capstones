using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileModulesManager : MonoBehaviour
    {
        [SerializeField]
        private ProfileHeaderFieldsManager profileHeaderFieldsManager;

        [SerializeField]
        private ProfileBiographyFieldsManager profileBiographyFieldsManager;

        private void OnEnable()
        {
            MainCanvasStateMachine.onAnimateToProfile.AddListener(SetContent);
        }

        private void OnDisable()
        {
            MainCanvasStateMachine.onAnimateToProfile.RemoveListener(SetContent);
        }

        public void SetContent()
        {
            profileHeaderFieldsManager.SetContent();
            profileBiographyFieldsManager.SetContent();
        }

    }
}


