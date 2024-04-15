using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using JoshKery.GenericUI.DOTweenHelpers;
using DG.Tweening;

namespace JoshKery.USGA.LockerCapstones
{
    public class ModulePaginatorManager : BaseWindow
    {
        [SerializeField]
        private VerticalScrollSnap scrollSnap;

        private int countScreens = 0;

        private AccomplishmentModal accomplishmentModal;

        private IEnumerator onPaginatorClickCoroutine = null;

        public delegate void OnModulesPageChange(int page);
        public static OnModulesPageChange onModulesPageChange;

        protected override void Awake()
        {
            base.Awake();

            accomplishmentModal = FindObjectOfType<AccomplishmentModal>();
        }

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

        public void OnPaginatorClick(int pageIndex)
        {
            if (onPaginatorClickCoroutine != null)
                return;

            onPaginatorClickCoroutine = OnPaginatorClickCoroutine(pageIndex);
            StartCoroutine(onPaginatorClickCoroutine);
        }

        private IEnumerator OnPaginatorClickCoroutine(int pageIndex)
        {
            if (accomplishmentModal.isOpen)
            {
                Tween close = accomplishmentModal.Close(SequenceType.UnSequenced);
                if (close == null) yield break;
                float duration = close.Duration();
                close.Kill();
                AccomplishmentModal.onClose?.Invoke();
                yield return new WaitForSeconds(duration + 0.05f);
            }

            onModulesPageChange?.Invoke(pageIndex);

            if (scrollSnap != null)
                scrollSnap.GoToScreen(pageIndex);

            yield return null;

            onPaginatorClickCoroutine = null;
        }
    }
}


