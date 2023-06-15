using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI;
using JoshKery.GenericUI.Accordion;

namespace JoshKery.USGA.LockerCapstones
{
    public class FilterButtonsManager : LockerCapstonesWindow
    {
        [SerializeField]
        private List<Transform> doNotDestroyChildTransforms;

        [SerializeField]
        private Transform insertAfterThisTransform;

        [SerializeField]
        private AccordionGroup accordionGroup;

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

                        child.parent = null; //equivalent of non-existent parent.DetachChild()
                        GameObject.Destroy(child.gameObject);
                    }
                }
            }
        }

        public override void SetContent()
        {
            if (appState == null) { return; }

            ClearAllDisplays();

            if (appState.data?.contentTrails != null)
            {
                int index = insertAfterThisTransform.GetSiblingIndex() + 1;
                foreach (ContentTrail contentTrail in appState.data.contentTrails)
                {
                    FilterOptionButton filterButton = InstantiateDisplay<FilterOptionButton>();
                    filterButton.gameObject.name = "FILTER BUTTON - " + contentTrail.name;
                    filterButton.SetContent(contentTrail);

                    filterButton.transform.SetSiblingIndex(index);
                    index++;
                }
            }
        }
    }

}


