using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.DOTweenHelpers.FlexibleUI;
using UnityEngine.EventSystems;

namespace JoshKery.USGA.LockerCapstones
{
    public class AccomplishmentModalCloseButton : ColorChangeButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (buttonWindow != null)
                buttonWindow.Close();
        }

        private void OnEnable()
        {
            AccomplishmentModal.onOpen += OnOpen;
        }

        private void OnDisable()
        {
            AccomplishmentModal.onOpen -= OnOpen;
        }

        private void OnOpen(Accomplishment accomplishment)
        {
            buttonWindow.Open(GenericUI.DOTweenHelpers.SequenceType.UnSequenced);
        }
    }
}


