using UnityEngine;

namespace NyanSnake
{
    internal class TutorialVisibility : GameUIVisibility
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private Tutorial _tutorial;

        #endregion // Inspector

        public override bool IsVisible() => !_tutorial.GetShown();
    }
}
