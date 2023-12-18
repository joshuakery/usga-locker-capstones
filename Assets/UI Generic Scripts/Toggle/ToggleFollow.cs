using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JoshKery.GenericUI.ToggleFollow
{
    [ExecuteAlways]
    public class ToggleFollow : MonoBehaviour
    {
        [SerializeField]
        private Toggle leader;

        [SerializeField]
        private Toggle follower;

        private void OnEnable()
        {
            if (leader != null && follower != null)
            {
                leader.onValueChanged.AddListener(ToggleFollower);
            }
        }

        private void OnDisable()
        {
            if (leader != null)
            {
                leader.onValueChanged.RemoveAllListeners();
            }
        }

        private void ToggleFollower(bool value)
        {
            if (follower != null)
                follower.isOn = value;
        }
    }
}


