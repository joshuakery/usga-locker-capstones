using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.USGA.Directus;
using TMPro;

namespace JoshKery.USGA.LockerCapstones
{
    public class MenuWindow : LockerCapstonesStateMachine
    {
        [SerializeField]
        private TMP_Text titleTextField;

        [SerializeField]
        private TMP_Text descriptionTextField;

        private Canvas canvas;

        protected override void Awake()
        {
            base.Awake();

            canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            if (canvas != null) { canvas.enabled = true; }
        }

        public override void SetContent()
        {
            if (appState == null) { return; }

            if (titleTextField != null)
            {
                titleTextField.text = appState.data?.era?.title;
            }

            if (descriptionTextField != null)
            {
                descriptionTextField.text = appState.data?.era?.description;
            }
        }
    }
}


