using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JoshKery.GenericUI.DOTweenHelpers.FlexibleUI
{
    [RequireComponent(typeof(FlexUIColorChangeWindow))]
    public class ColorChangeButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
    {
        /// <summary>
        /// The window to open and close
        /// </summary>
        protected FlexUIColorChangeWindow buttonWindow;

        protected virtual void Awake()
        {
            buttonWindow = GetComponent<FlexUIColorChangeWindow>();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (buttonWindow != null)
                buttonWindow.Close();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (buttonWindow != null)
                buttonWindow.Open();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (buttonWindow != null)
                buttonWindow.Close();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (buttonWindow != null)
                buttonWindow.Open();
        }
    }
}

