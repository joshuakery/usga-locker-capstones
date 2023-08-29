using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JoshKery.GenericUI.DOTweenHelpers;

namespace JoshKery.USGA.LockerCapstones
{
    /// <summary>
    /// Helper for custom scrolling behavior in profile modules.
    /// 
    /// Assumes scrollRect.content contains three RectTransforms:
    /// A top and bottom padding and a "contentMain" 
    /// </summary>
    public class ProfileModuleScrollHelper : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private RectTransform paddingTop;

        [SerializeField]
        private RectTransform contentMain;

        [SerializeField]
        private LayoutElement paddingBottomLE;

        /// <summary>
        /// The non-normalized maximum distance that scrollRect.content can move along the y-axis from pos = 0
        /// before the scrollRect script stops it
        /// </summary>
        [SerializeField]
        private float limit = 0f;

        /// <summary>
        /// The distance needed to scroll the contentMain up to the viewport top
        /// </summary>
        [SerializeField]
        private float delta = 0f;

        private void OnEnable()
        {
            ProfileModulesManager.onResetContent.AddListener(Setup);
        }

        private void OnDisable()
        {
            ProfileModulesManager.onResetContent.RemoveListener(Setup);
        }

        private void Setup(int id)
        {
            StartCoroutine(SetupCo());
        }

        private IEnumerator SetupCo()
        {
            yield return null; //yield here to wait for other functions hooked into ProfileModuleManager.onResetContent

            EnsurePaddingBottomMinimum();

            yield return null;

            ScrollContentMainToTopOfViewport();
        }

        private void EnsurePaddingBottomMinimum()
        {
            if (paddingBottomLE != null && scrollRect?.viewport != null && contentMain != null)
            {
                paddingBottomLE.preferredHeight = (scrollRect.viewport.rect.height - contentMain.rect.height);
            }
        }

        /// <summary>
        /// Since the content will by default scroll to the center of the viewport, and with padding,
        /// that might not be where we'd like the "contentMain" to land,
        /// we calculate the desired position and move the scrollRect.content to there.
        /// </summary>
        public void ScrollContentMainToTopOfViewport()
        {
            //Calculate the distance needed to scroll the contentMain up to the viewport top
            if (scrollRect?.viewport != null && scrollRect?.content != null && paddingTop != null)
            {
                limit = (scrollRect.content.rect.height - scrollRect.viewport.rect.height) / 2f;
                delta = Mathf.Min(limit, (scrollRect.viewport.rect.height / 2) - (scrollRect.content.rect.height / 2 - paddingTop.rect.height));
            }

            //Scroll
            scrollRect.content.anchoredPosition = new Vector2(0, delta);
        }
    }
}


