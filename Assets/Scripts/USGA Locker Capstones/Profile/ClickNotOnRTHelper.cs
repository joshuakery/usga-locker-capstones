using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JoshKery.USGA.LockerCapstones
{
    public class ClickNotOnRTHelper : MonoBehaviour
    {
        /// <summary>
        /// Fired when click is outside RT
        /// </summary>
        public UnityEvent onOutOfBounds;

        [SerializeField]
        Canvas targetCanvas;

        [SerializeField]
        LockerLocatorDrawer lockerLocatorDrawer;

        /// <summary>
        /// RT outside of which a click onPointerDown calls onOutOfBounds
        /// </summary>
        [SerializeField]
        RectTransform targetRectArea;

        [SerializeField]
        RectTransform[] exceptions;

        public bool doCheck = false;

        private void Update()
        {
            if (doCheck)
            {
                if (Input.GetMouseButtonDown(0) && targetRectArea != null && targetCanvas != null && targetCanvas.enabled
                    && lockerLocatorDrawer != null && lockerLocatorDrawer.isOpen)
                {
                    if (!RectTransformUtility.RectangleContainsScreenPoint(targetRectArea, Input.mousePosition) &&
                        !ExceptionsContainScreenPoint(Input.mousePosition))
                    {
                        onOutOfBounds.Invoke();
                    }
                }

            }

        }

        private bool ExceptionsContainScreenPoint(Vector2 point)
        {
            foreach (RectTransform rt in exceptions)
            {
                if (rt != null && RectTransformUtility.RectangleContainsScreenPoint(rt, point)) { return true; }
            }

            return false;
        }
    }

}

