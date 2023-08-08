using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JoshKery.GenericUI.DOTweenHelpers;
using JoshKery.GenericUI.Carousel;


namespace JoshKery.USGA.LockerCapstones
{
    public class AttractStateMachine : LockerCapstonesStateMachine
    {
        [SerializeField]
        private TMP_Text titleField;

        [SerializeField]
        private TMP_Text dateField;

        [SerializeField]
        private SlideManager slideManager;

        public override void SetContent()
        {
            if (appState == null) { return; }

            if (titleField != null)
            {
                titleField.text = appState.data?.era?.title;
            }

            if (dateField != null)
            {
                string dateString = string.Format(
                    "{0} to {1}",
                    appState.data?.era?.startYear.ToString(), appState.data?.era?.endYear.ToString()
                );
                dateField.text = dateString;
            }

            /*if (slideManager != null)
            {
                slideManager.InitializeWithExistingChildren();
            }*/
        }
    }
}


