using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JoshKery.GenericUI
{
    [RequireComponent(typeof(Button))]
    public class MyButtonHelper : MonoBehaviour
    {
        private Button button;

        private static float notInteractableTimeout = 0.5f;

        [SerializeField]
        private bool doAutoSetInteractableAgain = true;

        private EventSystem eventSystem;

        private void Awake()
        {
            if (button == null)
                button = GetComponent<Button>();

            eventSystem = FindObjectOfType<EventSystem>();
        }

        private void OnEnable()
        {
            if (button != null)
            {
                button.onClick.AddListener(BrieflySetNotInteractableButton);
                button.onClick.AddListener(Deselect);
            }
        }

        private void OnDisable()
        {
            if (button != null)
            {
                button.onClick.RemoveListener(BrieflySetNotInteractableButton);
                button.onClick.RemoveListener(Deselect);
            }
        }

        private void BrieflySetNotInteractableButton()
        {
            StartCoroutine(BrieflySetNotInteractableButtonCo());
        }

        private IEnumerator BrieflySetNotInteractableButtonCo()
        {
            if (button != null)
            {
                button.interactable = false;

                yield return new WaitForSeconds(notInteractableTimeout);

                if (doAutoSetInteractableAgain)
                    button.interactable = true;
            }
        }

        private void Deselect()
        {
            if (eventSystem != null)
                eventSystem.SetSelectedGameObject(null);
        }

    }
}


