using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.Attract;

namespace JoshKery.USGA.LockerCapstones
{
    public class LockerCapstonesAttractTimeout : AttractTimeout
    {
        protected override void OnTimeout()
        {
            MainCanvasStateMachine.onAnimateToAttract?.Invoke();
        }
    }
}


