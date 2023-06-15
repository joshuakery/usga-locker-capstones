using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using JoshKery.GenericUI;

namespace JoshKery.USGA.LockerCapstones
{
    public class FilterOptionButton : BaseDisplay
    {
        public delegate void FilterClickedDelegate(int contentTrailID);
        public static FilterClickedDelegate onFilterClicked;

        public int contentTrailID;

        [SerializeField]
        private TMP_Text nameField;

        public void OnFilterClicked()
        {
            onFilterClicked(contentTrailID);
        }

        public void SetContent(ContentTrail contentTrail)
        {
            if (contentTrail != null)
            {
                contentTrailID = contentTrail.id;

                if (nameField != null)
                {
                    nameField.text = contentTrail.name;
                }
            }
        }
    }

}

