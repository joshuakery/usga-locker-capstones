using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.Extensions;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class ModulesPaginator : BaseWindow
    {
        [SerializeField]
        private TMP_Text textDisplay;

        [SerializeField]
        private TMP_Text checkmarkTextDisplay;

        [SerializeField]
        private Button button;

        private VerticalScrollSnap scrollSnap;

        public void SetContent(string text, VerticalScrollSnap vss)
        {
            textDisplay.text = text;
            checkmarkTextDisplay.text = text;

            scrollSnap = vss;

            if (button != null)
                button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveAllListeners();
        }

        private void OnClick()
        {
            if (scrollSnap != null)
            {
                scrollSnap.GoToScreen(transform.GetSiblingIndex());
            }
        }
    }
}


