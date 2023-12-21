using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace JoshKery.GenericUI.Attract
{
    public class AttractTimeout : MonoBehaviour
    {
        /// <summary>
        /// Seconds
        /// </summary>
        public int timeoutDuration = 60;

        bool anyClick = false;
        float timeSinceLastClick = 0.0f;

        bool timedOut = false;

        private void Update()
        {
            timeSinceLastClick += Time.deltaTime;

            if (Input.anyKey ||
                Input.GetMouseButton(0) ||
                Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                anyClick = true;
            }

            if (anyClick)
            {
                timeSinceLastClick = 0.0f;
                anyClick = false;
            }

            if (timeSinceLastClick > timeoutDuration && !timedOut)
            {
                OnTimeout();

                timedOut = true;
            }
            else if (timeSinceLastClick < timeoutDuration && timedOut)
            {
                timedOut = false;
            }
        }

        protected virtual void OnTimeout()
        {

        }

        public void ResetTimer()
        {
            timeSinceLastClick = 0f;
        }
    }
}

