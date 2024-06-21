using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NyanSnake
{
    internal class GameUI : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private GameUIVisibility _visibility;

        [Header("Settings")]

        [SerializeField] private bool _hasForcedScale;
        [SerializeField] private Vector3 _forcedScale;
        [SerializeField] private bool _hasCustomMargin;
        [SerializeField] private Vector2 _customMargin;

        #endregion // Inspector

        private Dictionary<(Vector3, Vector2), Rect> _visibleRectCache = new();

        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public bool TryGetVisibleRect(out Rect rect, Vector2 margin = default)
        {
            rect = default;
            bool visible = (_visibility ? _visibility.IsVisible() : isActiveAndEnabled);
            if (visible)
            {
                if (_hasCustomMargin)
                {
                    margin = _customMargin;
                }
                if (!_visibleRectCache.TryGetValue((_rectTransform.position, margin), out rect))
                {
                    Action revertScale = null;
                    if (_hasForcedScale)
                    {
                        Utilities.SetGlobalScale(_rectTransform, _forcedScale, ref revertScale);
                    }
                    rect = _visibleRectCache[(_rectTransform.position, margin)] = Utilities.GetVisibleRect(_rectTransform, margin);
                    revertScale?.Invoke();
                }
            }
            return visible;
        }

        public static bool Contains(IEnumerable<GameUI> gameUis, Vector3 position, Vector2 margin = default)
        {
            Rect rect;
            return gameUis.Any(gameUi => gameUi.TryGetVisibleRect(out rect, margin) && rect.Contains(position));
        }

        public static void DrawVisibleRectGizmo(IEnumerable<GameUI> gameUis, Vector2 margin = default)
        {
            foreach (GameUI gameUi in gameUis)
            {
                Rect rect;
                if (gameUi && gameUi.TryGetVisibleRect(out rect, margin))
                {
                    Gizmos.DrawWireCube(rect.center, rect.size);
                }
            }
        }
    }
}
