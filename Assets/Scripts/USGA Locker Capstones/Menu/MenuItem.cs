using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class MenuItem : BaseWindow
    {
        public List<string> categories;

        public void OnClick()
        {
            MainCanvasStateMachine.onAnimateToProfile.Invoke();
        }
    }
}


