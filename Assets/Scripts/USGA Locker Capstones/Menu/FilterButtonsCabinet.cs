using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace JoshKery.USGA.LockerCapstones
{
    public class FilterButtonsCabinet : LockerCapstonesWindow
    {
        [SerializeField]
        private LeTai.TrueShadow.TrueShadow trueShadow;

        protected override void OnEnable()
        {
            base.OnEnable();

            MainCanvasStateMachine.beforeAnimateToMenu += CloseAndComplete;
            MainCanvasStateMachine.onAnimateToProfile.AddListener(OnAnimateToProfile);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            MainCanvasStateMachine.beforeAnimateToMenu -= CloseAndComplete;
            MainCanvasStateMachine.onAnimateToProfile.RemoveListener(OnAnimateToProfile);
        }

        private void OnAnimateToProfile(int id)
        {
            _Close(GenericUI.DOTweenHelpers.SequenceType.UnSequenced);
        }

        private void CloseAndComplete()
        {
            Close(GenericUI.DOTweenHelpers.SequenceType.CompleteImmediately);
        }

        public override void SetContent()
        {
            if (appState == null) { return; }

            ClearAllDisplays();

            if (appState.data?.contentTrails != null)
            {
                foreach (ContentTrail contentTrail in appState.data.contentTrails)
                {
                    if (contentTrail != null)
                    {
                        FilterDrawer filterButton = InstantiateDisplay<FilterDrawer>();
                        filterButton.gameObject.name = "FILTER BUTTON - " + contentTrail.name;
                        filterButton.SetContent(contentTrail);

                        if (appState?.data?.era?.contentTrailIDs != null)
                        {
                            filterButton.isInEra = appState.data.era.contentTrailIDs.Contains(contentTrail.id);
                        }
                        filterButton.SetInteractable().Complete();
                    }
                }
            }

            ResetChildWindows();

            StartCoroutine(ToggleTrueShadow());
        }

        /// <summary>
        /// Seeing a bug in this third party script that throws an error every frame because child gameobjects were deleted.
        /// Toggling it seems to fix the problem.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ToggleTrueShadow()
        {
            if (trueShadow != null)
                trueShadow.enabled = false;

            yield return null;

            if (trueShadow != null)
                trueShadow.enabled = true;
        }
    }
}


