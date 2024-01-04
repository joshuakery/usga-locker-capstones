using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.AspectRatio;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileBackgroundManager : MonoBehaviour
    {
        [SerializeField]
        private AppState appState;

        [SerializeField]
        private RawImageManager riManager;

        public void SetRandom()
        {
            if (appState != null && appState.profileBackgrounds.Count > 0)
            {
                int index = Random.Range(0, appState.profileBackgrounds.Count);
                riManager.texture = appState.profileBackgrounds[index];
            }
        }
    }
}


