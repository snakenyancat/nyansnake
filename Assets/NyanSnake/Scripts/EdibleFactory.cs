using System.Collections.Generic;
using UnityEngine;

namespace NyanSnake
{
    internal class EdibleFactory : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private List<Edible> _prefabs;
        [SerializeField] private Body _body;
        [SerializeField] private List<GameUI> _gameUis;

        [Header("Settings")]

        [SerializeField] private float _margin;
        [SerializeField] private float _gameUiMargin = .375f;
        [SerializeField] private float _bodyDistance;

        #endregion // Inspector

        private Vector2 GameUiMargin => Vector2.one * _gameUiMargin;

        public Edible Create()
        {
            Edible ediblePrefab = _prefabs[Random.Range(0, _prefabs.Count)];
            Rect rectWithMargin = Utilities.GetVisibleRect(_margin, safeArea: true);
            Vector3 position;
            do
            {
                position = new Vector2(Random.Range(rectWithMargin.xMin, rectWithMargin.xMax), Random.Range(rectWithMargin.yMin, rectWithMargin.yMax));
            } while ((position - _body.transform.position).sqrMagnitude <= _bodyDistance * _bodyDistance || GameUI.Contains(_gameUis, position, GameUiMargin));
            return Instantiate(ediblePrefab, position, Quaternion.identity);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            GameUI.DrawVisibleRectGizmo(_gameUis, GameUiMargin);
        }
    }
}
