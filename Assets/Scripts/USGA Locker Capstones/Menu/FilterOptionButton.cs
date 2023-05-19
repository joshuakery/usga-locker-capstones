using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JoshKery.USGA.LockerCapstones
{
    public class FilterOptionButton : MonoBehaviour
    {
        public delegate void FilterClickedDelegate(string category);
        public static FilterClickedDelegate onFilterClicked;

        public string category;

        public void OnFilterClicked()
        {
            onFilterClicked(category);
        }
    }

}

