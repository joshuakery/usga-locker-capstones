using UnityEngine;
using JoshKery.FlexibleUI;

namespace JoshKery.GenericUI.DOTweenHelpers.FlexibleUI
{
    /// <summary>
    /// Used to extend the AnimationData scriptable object for use of FlexUI choices
    /// </summary>
    [CreateAssetMenu(fileName = "UIAnimation - FlexUI", menuName = "UI Animation - Flex UI")]
    public class FlexUIColorChangeAnimationData : UIAnimationData
    {
        public enum ExtendedAnimationType
        {
            Ignore = 0,
            Color = 1,
            VertexColor = 2
        }

        /// <summary>
        /// We use this to 'extend' the AnimationType enum.
        /// When this scriptable object is used, the using class should check if
        /// 'Ignore' has been chosen here. If so, it should use one of the default
        /// AnimationTypes. Else, it should use one of the extensions.
        /// </summary>
        public ExtendedAnimationType extendedAnimationType = ExtendedAnimationType.Ignore;

        /// <summary>
        /// Data that maps ColorChoices to Color32
        /// </summary>
        [SerializeField]
        private FlexibleUIData skinData;

        /// <summary>
        /// ColorChoice from which the tween will animate
        /// </summary>
        public ColorChoice fromColorChoice;

        /// <summary>
        /// ColorChoice to which the tween will animate
        /// </summary>
        public ColorChoice toColorChoice;

        /// <summary>
        /// Helper method to grab the color from skinData
        /// </summary>
        /// <param name="colorChoice">Which color to grab. See mapping in FlexibleUIData.cs</param>
        /// <returns>Color32 corresponding to colorChoice</returns>
        private Color32 GetColor(ColorChoice colorChoice)
        {
            if (skinData != null)
                return skinData.GetColor(colorChoice);
            else
                return Color.white;
        }

        public Color32 fromColor
        {
            get
            {
                return GetColor(fromColorChoice);
            }
        }

        public Color32 toColor
        {
            get
            {
                return GetColor(toColorChoice);
            }
        }
    }
}


