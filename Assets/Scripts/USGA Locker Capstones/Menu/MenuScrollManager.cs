using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JoshKery.USGA.LockerCapstones
{
    [RequireComponent(typeof(ScrollRect))]
    public class MenuScrollManager : MonoBehaviour
    {
        private ScrollRect scrollRect;

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        private void OnEnable()
        {
            MenuItemManager.onFiltered.AddListener(ScrollToTop);

            MainCanvasStateMachine.onAnimateToMenu.AddListener(ScrollToTop);
        }

        private void OnDisable()
        {
            MenuItemManager.onFiltered.RemoveListener(ScrollToTop);

            MainCanvasStateMachine.onAnimateToMenu.RemoveListener(ScrollToTop);
        }

        private void ScrollToTop()
        {
            StartCoroutine(ScrollCo(1f));
        }

        private void ScrollToBottom()
        {
            StartCoroutine(ScrollCo(0f));
        }

        /// <summary>
        /// Yield a frame here so that the scrollrect can regenerate
        /// In case the content size has changed.
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        private IEnumerator ScrollCo(float to)
        {
            yield return null;
            scrollRect.verticalNormalizedPosition = to;
        }
    }
}


