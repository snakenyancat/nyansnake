using Gists;
using System.Collections.Generic;
using UnityEngine;

namespace NyanSnake
{
    internal class StarField : MonoBehaviour, ICollidable
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private Star _starPrefab;
        [SerializeField] private EdgeCollider2D _edgeCollider2D;
        [SerializeField] private List<GameUI> _gameUis;

        [Header("Settings")]

        [SerializeField] private float _starDistance = 2;
        [SerializeField] private float _margin = .1f;
        [SerializeField] private float _gameUiMargin = .25f;
        [SerializeField] private int _iterationPerPoint = 45;

        #endregion // Inspector

        private Vector2 GameUiMargin => Vector2.one * _gameUiMargin;

        private void Start()
        {
            // Adds stars.
            Rect rectWithMargin = Utilities.GetVisibleRect(_margin);
            List<Vector2> starPositions = FastPoissonDiskSampling.Sampling(rectWithMargin.min, rectWithMargin.max, _starDistance, _iterationPerPoint);
            foreach (Vector2 starPosition in starPositions)
            {
                if (!GameUI.Contains(_gameUis, starPosition, GameUiMargin))
                {
                    Instantiate(_starPrefab, starPosition, Quaternion.identity, transform);
                }
            }
            // Sets the collider points.
            Rect rect = Utilities.GetVisibleRect();
            _edgeCollider2D.points = new Vector2[] { rect.min, new Vector2(rect.xMin, rect.yMax), rect.max, new Vector2(rect.xMax, rect.yMin), rect.min };
            // Aplies the skin.
            ApplySkin();
            SkinDressing.Instance.OnSkinChanged += ApplySkin;
        }

        private void ApplySkin()
        {
            Skin skin = SkinDressing.Instance.GetSkin();
            Camera.main.backgroundColor = skin.StarFieldColor;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            GameUI.DrawVisibleRectGizmo(_gameUis, GameUiMargin);
        }
    }
}
