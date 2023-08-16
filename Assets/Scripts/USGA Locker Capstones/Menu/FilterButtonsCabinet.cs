using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace JoshKery.USGA.LockerCapstones
{
    public class FilterButtonsCabinet : LockerCapstonesWindow
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            MainCanvasStateMachine.beforeAnimateToMenu += CloseAndComplete;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            MainCanvasStateMachine.beforeAnimateToMenu -= CloseAndComplete;
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
                            filterButton.button.interactable = appState.data.era.contentTrailIDs.Contains(contentTrail.id);
                        }
                        filterButton.SetInteractable(filterButton.button.interactable).Complete();
                    }
                }
            }

            ResetChildWindows();
        }
    }
}


