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
                mainCanvasState.AnimateToAttract();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                sequenceManager.CompleteCurrentSequence();
                mainCanvasState.AnimateToIntro();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                sequenceManager.CompleteCurrentSequence();
                mainCanvasState.AnimateToMenu();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                sequenceManager.CompleteCurrentSequence();
                mainCanvasState.AnimateToProfile();
            }
        }
    }
}

