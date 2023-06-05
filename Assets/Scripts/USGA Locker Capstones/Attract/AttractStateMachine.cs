using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JoshKery.GenericUI.DOTweenHelpers;


namespace JoshKery.USGA.LockerCapstones
{
    public class AttractStateMachine : LockerCapstonesStateMachine
    {
        [SerializeField]
        private TMP_Text titleField;

        [SerializeField]
        private TMP_Text dateField;

        public override void SetContent()
        {
            if (titleField != null)
            {
                //so how will eras work? will they be like designated on each profile, or will each era have a list of profiles?
            }
        }
    }
}


