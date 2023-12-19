using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class UIDebugHelper : MonoBehaviour
    {
        [SerializeField]
        private UISequenceManager sequenceManager;

        [SerializeField]
        private MainCanvasStateMachine mainCanvasState;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                sequenceManager.CompleteCurrentSequence();
                BaseWindow.onAwakeWindows.Invoke();
                BaseWindow.onStartUpWindows.Invoke();
                BaseStateMachine.onStartUpStateMachines.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                sequenceManager.CompleteCurrentSequence();
                MainCanvasStateMachine.onAnimateToAttract?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                sequenceManager.CompleteCurrentSequence();
                MainCanvasStateMachine.onAnimateToIntro?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                sequenceManager.CompleteCurrentSequence();
                MainCanvasStateMachine.onAnimateToMenu?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                sequenceManager.CompleteCurrentSequence();
                MainCanvasStateMachine.onAnimateToProfile?.Invoke(0); //accepts locker profile id as input
            }
        }
    }
}

