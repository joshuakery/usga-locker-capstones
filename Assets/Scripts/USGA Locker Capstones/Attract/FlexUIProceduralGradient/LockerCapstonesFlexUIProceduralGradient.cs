using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoshH.UI;
using JoshKery.FlexibleUI;

namespace JoshKery.USGA.LockerCapstones
{
    [RequireComponent(typeof(UIGradient))]
    public class LockerCapstonesFlexUIProceduralGradient : FlexibleUIBase
    {
        private UIGradient gradientComponent;

        [SerializeField]
        private ColorChoice[] colorChoices;

        public override void Awake()
        {
            base.Awake();

            gradientComponent = GetComponent<UIGradient>();
        }

        protected override void OnSkinUI()
        {
            if (skinData == null) { return; }

            if (colorChoices != null && gradientComponent != null)
            {
                for (int i=0; i<colorChoices.Length; i++)
                {
                    if (gradientComponent.LinearGradient.colorKeys.Length >= i + 1)
                    {
                        ColorChoice colorChoice = colorChoices[i];
                        if (colorChoice != ColorChoice.Custom)
                        {
                            GradientColorKey[] keys = gradientComponent.LinearGradient.colorKeys;
                            keys[i].color = skinData.GetColor(colorChoice);
                            gradientComponent.LinearGradient.colorKeys = keys;
                        }
                    }
                }

                gradientComponent.ForceUpdateGraphic();
            }

            base.OnSkinUI();
        }
    }
}


