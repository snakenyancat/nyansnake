using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using static NyanSnake.Sound;

namespace NyanSnake
{
    [DefaultExecutionOrder(ExecutionOrder)]
    internal class Body : MonoBehaviour
    {
        public const int ExecutionOrder = 0;

        #region Inspector

        [Header("References")]

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;
        [SerializeField] private PolygonCollider2D _polygonCollider2D;
        [SerializeField] private List<GameUI> _touchMask;
        [SerializeField] private Game _game;

        [Header("Settings")]

        [SerializeField] private float _speed = 1;
        [SerializeField] private float _lastSegmentLength = 10;
        [SerializeField] private float _waypointDistance = 1;
        [SerializeField] private float _knotsMinDistance = .1f;

        #endregion // Inspector

        private float _turnRadius;
        private float _tangentLength;
        private int _turnExitKnotIndex;
        private int _knotIndex;

        public event Action<ICollidable> OnCollision;

        public Spline Spline { get; private set; }
        public float Distance { get; private set; }
        public int Edibles { get; set; }

        private void Start()
        {
            _turnRadius = Tail.Tickness;
            _tangentLength = _turnRadius / 2;
            _turnExitKnotIndex = _knotIndex = 0;
            Rect safeArea = Utilities.GetVisibleRect(safeArea: true);
            transform.position = new Vector3(safeArea.xMin + 1, 0, 0);
            Spline = new Spline()
            {
                new BezierKnot() { Position = transform.position},
                new BezierKnot() { Position = transform.position + transform.right * _lastSegmentLength},
            };
            Distance = 0;
            Edibles = 0;
            ApplySkin();
            SkinDressing.Instance.OnSkinChanged += ApplySkin;
        }

        private void ApplySkin()
        {
            Skin skin = SkinDressing.Instance.GetSkin();
            _animator.runtimeAnimatorController = skin.BodyAnimationController;
        }

        private void Update()
        {
            // Updates the spline from the touches.
            if (_knotIndex >= _turnExitKnotIndex && Input.GetMouseButtonDown(0) && !_game.IsPaused)
            {
                Vector3 touchWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (!GameUI.Contains(_touchMask, touchWorldPosition))
                {
                    Sound.Instance.PlayEffect(Effect.Turn);
                    int knotsToSkip = _knotIndex > _turnExitKnotIndex && (transform.position - (Vector3)Spline[^2].Position).sqrMagnitude < _knotsMinDistance * _knotsMinDistance ? 2 : 1;
                    Vector3 newDirection = (Vector3.Dot(touchWorldPosition - transform.position, transform.up) > 0 ? transform.up : -transform.up);
                    Spline.Knots = Spline.Knots.SkipLast(knotsToSkip).Concat(new[]
                    {
                        new BezierKnot() { Position = transform.position, TangentOut = transform.right * _tangentLength }, // XXX Merge with previous node if they are too close.
                        new BezierKnot() { Position = transform.position + (transform.right + newDirection) * _turnRadius, TangentIn =  -newDirection * _tangentLength},
                        new BezierKnot() { Position = transform.position + (transform.right + newDirection) * _turnRadius + newDirection * _lastSegmentLength},
                    });
                    _knotIndex = Spline.Knots.Count() - 3;
                    _turnExitKnotIndex = Spline.Knots.Count() - 2;
                }
            }
            // Updates the spline with waypoints.
            if (_knotIndex >= _turnExitKnotIndex && (transform.position - (Vector3)Spline[^2].Position).sqrMagnitude > _waypointDistance * _waypointDistance)
            {
                BezierKnot newKnot = new BezierKnot() { Position = transform.position };
                List<BezierKnot> knots = new(Spline.Knots);
                knots.Insert(knots.Count - 1, newKnot);
                Spline.Knots = knots;
            }
            // Updates the movement along the spline.
            Distance += Time.deltaTime * _speed;
            float t = Distance / Spline.GetLength();
            _knotIndex = SplineUtility.SplineToCurveT(Spline, t, out _);
            float3 position;
            float3 tangent;
            Spline.Evaluate(t, out position, out tangent, out _);
            transform.position = position;
            transform.right = tangent;
            if (Vector3.Dot(transform.forward, Vector3.forward) < 0)
            {
                Vector3 eulerAngles = transform.eulerAngles;
                eulerAngles.x = 180;
                transform.eulerAngles = eulerAngles;
            }
        }

        private void LateUpdate()
        {
            // Updates the collider of the cat.
            List<Vector2> points = new();
            _spriteRenderer.sprite.GetPhysicsShape(0, points);
            _polygonCollider2D.points = points.ToArray();
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            ICollidable collidable;
            if (collider2D.TryGetComponent<ICollidable>(out collidable))
            {
                OnCollision?.Invoke(collidable);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Utilities.DrawGizmoSpline(Spline);
            Gizmos.color = Color.blue;
            GameUI.DrawVisibleRectGizmo(_touchMask);
        }
    }
}
