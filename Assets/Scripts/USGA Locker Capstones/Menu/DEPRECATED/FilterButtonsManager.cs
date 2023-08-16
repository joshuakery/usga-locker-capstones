using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JoshKery.GenericUI;
using JoshKery.GenericUI.Accordion;

namespace JoshKery.USGA.LockerCapstones
{
    public class FilterButtonsManager : AccordionGroup
    {
        [SerializeField]
        private AppState appState;

        [SerializeField]
        private List<Transform> doNotDestroyChildTransforms;

        [SerializeField]
        private Transform insertAfterThisTransform;

        private static UnityEvent _onSetContent;
        public static UnityEvent onSetContent
        {
            get
            {
                if (_onSetContent == null)
                    _onSetContent = new UnityEvent();

                return _onSetContent;
            }
        }

        #region MonoBehaviour Methods
        protected override void OnEnable()
        {
            base.OnEnable();

            onSetContent.AddListener(SetContent);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            onSetContent.RemoveListener(SetContent);
        }
        #endregion

        public override void ClearAllDisplays()
        {
            if (childDisplaysContainer != null)
            {
                foreach (Transform child in childDisplaysContainer)
                {
                    if (!doNotDestroyChildTransforms.Contains(child))
                    {
                        BaseDisplay childDisplay = child.gameObject.GetComponent<BaseDisplay>();
                        if (childDisplay != null) { childDisplays.Remove(childDisplay); }

                        child.SetParent(null, false); //equivalent of non-existent parent.DetachChild()
                        GameObject.Destroy(child.gameObject);
                    }
                }
            }
        }

        public void SetContent()
        {
            if (appState == null) { return; }

            ClearAllDisplays();

            if (appState?.data?.contentTrails != null)
            {
                int index = insertAfterThisTransform.GetSiblingIndex() + 1;
                foreach (ContentTrail contentTrail in appState.data.contentTrails)
                {
                    if (contentTrail != null)
                    {
                        FilterOptionButton filterButton = InstantiateDisplay<FilterOptionButton>();
                        filterButton.gameObject.name = "FILTER BUTTON - " + contentTrail.name;
                        filterButton.SetContent(contentTrail);

                        filterButton.button.interactable = appState.data.era.contentTrailIDs.Contains(contentTrail.id);

                        filterButton.transform.SetSiblingIndex(index);
                        index++;
                    }
                }
            }

            ResetChildWindows();
        }
    }

}


