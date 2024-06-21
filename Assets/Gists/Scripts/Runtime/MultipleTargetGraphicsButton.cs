using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gists
{
    /// <summary>
    /// Stands for a button with multiple target graphics.
    /// See https://forum.unity.com/threads/tint-multiple-targets-with-single-button.279820/#post-5092682.
    /// </summary>
    public class MultipleTargetGraphicsButton : Button
    {
        [SerializeField]
        private List<Graphic> _additionalTargetGraphics;

        public List<Graphic> AdditionalTargetGraphics
        {
            get => _additionalTargetGraphics;
            set => _additionalTargetGraphics = value;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            var targetColor =
                state == SelectionState.Disabled ? colors.disabledColor :
                state == SelectionState.Highlighted ? colors.highlightedColor :
                state == SelectionState.Normal ? colors.normalColor :
                state == SelectionState.Pressed ? colors.pressedColor :
                state == SelectionState.Selected ? colors.selectedColor : Color.white;
            foreach (Graphic targetGraphic in _additionalTargetGraphics)
            {
                targetGraphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
            }
        }
    }
}
