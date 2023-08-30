using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshKery.GenericUI.Carousel;

namespace JoshKery.USGA.LockerCapstones
{
    public class ProfileModulesCarousel : Carousel
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            ProfileModulesManager.onResetContent.AddListener(OnResetContent);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            ProfileModulesManager.onResetContent.RemoveListener(OnResetContent);
        }

        private void OnResetContent(int profileID)
        {
            GoToFirstSlide();
            sequenceManager.CompleteCurrentSequence();
        }
    }
}


