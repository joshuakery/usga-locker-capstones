using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JoshKery.USGA.LockerCapstones
{
    public class FilterButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private FilterButtonsCabinet filterButtonsCabinet;

        [SerializeField]
        private MenuItemManager menuItemManager;

        private void OnEnable()
        {
            if (button != null)
                button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            if (button != null)
                button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (filterButtonsCabinet != null && menuItemManager != null)
            {
                if (menuItemManager.selectedContentTrailIDs != null &&
                    !menuItemManager.selectedContentTrailIDs.Contains(-1)
                    )
                {
                    FilterDrawer.onFilterClicked(-1); //remove all filters
                }

                filterButtonsCabinet.DoToggle();
            }
        }
    }
}


