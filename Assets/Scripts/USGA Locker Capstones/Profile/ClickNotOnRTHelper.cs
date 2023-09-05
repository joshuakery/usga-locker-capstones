using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickNotOnRTHelper : MonoBehaviour
{
    /// <summary>
    /// Fired when click is outside RT
    /// </summary>
    public UnityEvent onOutOfBounds;

    /// <summary>
    /// RT outside of which a click onPointerDown calls onOutOfBounds
    /// </summary>
    [SerializeField]
    RectTransform targetRectArea;

    public bool doCheck = false;

    private void Update()
    {
        if (doCheck)
        {
            if (Input.GetMouseButtonDown(0) && targetRectArea != null)
            {
                Debug.Log("checking");
                if (!RectTransformUtility.RectangleContainsScreenPoint(targetRectArea, Input.mousePosition))
                {
                    onOutOfBounds.Invoke();
                    Debug.Log("out!");
                }
            }
            
        }
        
    }
}
