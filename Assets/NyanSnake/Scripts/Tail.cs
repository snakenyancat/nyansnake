using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;
using Spline = UnityEngine.Splines.Spline;
using U2DSpline = UnityEngine.U2D.Spline;

namespace NyanSnake
{
    [DefaultExecutionOrder(ExecutionOrder)]
    internal class Tail : MonoBehaviour, ICollidable
    {
        public const int ExecutionOrder = Body.ExecutionOrder + 1;
        public const float Tickness = .35f;

        #region Inspector

        [Header("References")]

        [SerializeField] private SpriteShapeController _spriteShapeController;
        [SerializeField] private EdgeCollider2D _edgeCollider2D;
        [SerializeField] private Body _body;

        [Header("Settings")]

        [SerializeField] private float _colliderEndOffset = 2;
        [SerializeField] private float _colliderSegmentLength = .5f;

        #endregion // Inspector

        private float _colliderStartOffset;
        private float _length;

        private void Start()
        {
            _colliderStartOffset = Tickness;
            _length = 0;
            ApplySkin();
            SkinDressing.Instance.OnSkinChanged += ApplySkin;
        }

        private void ApplySkin()
        {
            Skin skin = SkinDressing.Instance.GetSkin();
            _spriteShapeController.spriteShape = skin.TailSpriteShape;
        }

        private void Update()
        {
            if (_body.Distance < _body.Edibles)
            {
                return;
            }
            _length = Mathf.Lerp(_length, _body.Edibles, Time.deltaTime / 2);
            float bodySplineLength = _body.Spline.GetLength();
            // Processes the tail spline.
            float startT = (_body.Distance - _length) / bodySplineLength;
            float startTCurve;
            int startCurveIndex = _body.Spline.SplineToCurveT(startT, out startTCurve);
            float endT = _body.Distance / bodySplineLength;
            float endTCurve;
            int endCurveIndex = _body.Spline.SplineToCurveT(endT, out endTCurve);
            List<BezierKnot> knots = new();
            for (int curveIndex = startCurveIndex, knotIndex = 0; curveIndex <= endCurveIndex; curveIndex++, knotIndex++)
            {
                BezierCurve curve = _body.Spline.GetCurve(curveIndex);
                if (startCurveIndex == endCurveIndex)
                {
                    CurveUtility.Split(curve, startTCurve, out _, out curve);
                    CurveUtility.Split(curve, (endTCurve - startTCurve) / (1 - startTCurve), out curve, out _);
                }
                else if (curveIndex == startCurveIndex)
                {
                    CurveUtility.Split(curve, startTCurve, out _, out curve);
                }
                else if (curveIndex == endCurveIndex)
                {
                    CurveUtility.Split(curve, endTCurve, out curve, out _);
                }
                if (Vector3.SqrMagnitude(curve.P0 - curve.P3) < .001f /* SpriteShapeController distance tolerance */)
                {
                    continue;
                }
                T RemoveLast<T>(List<T> list)
                {
                    T t = list[^1];
                    list.Remove(t);
                    return t;
                }
                BezierKnot startKnot = knots.Count == 0 ? new BezierKnot() : RemoveLast(knots);
                startKnot.Position = curve.P0;
                startKnot.TangentOut = curve.Tangent0;
                knots.Add(startKnot);
                BezierKnot endKnot = new BezierKnot();
                endKnot.Position = curve.P3;
                endKnot.TangentIn = curve.Tangent1;
                knots.Add(endKnot);
            }
            Spline spline = new Spline(knots);
            // Updates the rendering of the tail.
            U2DSpline u2dSpline = _spriteShapeController.spline;
            u2dSpline.Clear();
            for (int i = 0; i < spline.Count; i++)
            {
                u2dSpline.InsertPointAt(i, spline[i].Position);
                u2dSpline.SetCorner(i, true);
                u2dSpline.SetTangentMode(i, ShapeTangentMode.Broken);
                u2dSpline.SetRightTangent(i, spline[i].TangentOut);
                u2dSpline.SetLeftTangent(i, spline[i].TangentIn);
            }
            // Updates the collider of the tail.
            float colliderT = (_body.Distance - _length + _colliderStartOffset) / bodySplineLength;
            float colliderEndT = (_body.Distance - _colliderEndOffset) / bodySplineLength;
            float cColliderTIncrement = _colliderSegmentLength / bodySplineLength;
            if (colliderT <= colliderEndT)
            {
                Vector2 GetColliderPoint(float colliderT) => (Vector3)_body.Spline.EvaluatePosition(colliderT);
                List<Vector2> colliderPoints = new() { GetColliderPoint(colliderT) };
                do
                {
                    colliderT = Mathf.Clamp(colliderT + cColliderTIncrement, 0, colliderEndT);
                    colliderPoints.Add(GetColliderPoint(colliderT));
                } while (colliderT < colliderEndT);
                _edgeCollider2D.points = colliderPoints.ToArray();
            }
        }
    }
}
