using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.Extensions;
using JoshKery.GenericUI.DOTweenHelpers;
using DG.Tweening;

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



        private ModulePaginatorManager manager;

        protected override void Awake()
        {
            base.Awake();

            manager = FindObjectOfType<ModulePaginatorManager>();
        }

        public void SetContent(string text, VerticalScrollSnap vss)
        {
            textDisplay.text = text;
            checkmarkTextDisplay.text = text;

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
            if (manager != null)
                manager.OnPaginatorClick(transform.GetSiblingIndex());
        }
    }
}


