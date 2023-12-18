using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    public class ModulePaginatorManager : BaseWindow
    {
        [SerializeField]
        private VerticalScrollSnap scrollSnap;

        private int countScreens = 0;

        public void ResetScroll()
        {
            scrollSnap.StartingScreen = countScreens - 1;
            scrollSnap.GoToScreen(countScreens - 1);
            scrollSnap.UpdateLayout();
        }

        public void InstantiatePaginator(string text)
        {
            ModulesPaginator paginator = InstantiateDisplay<ModulesPaginator>();
            paginator.SetContent(text, scrollSnap);
            countScreens += 1;
        }

        public override void ClearAllDisplays()
        {
            base.ClearAllDisplays();
            countScreens = 0;
        }
    }
}


