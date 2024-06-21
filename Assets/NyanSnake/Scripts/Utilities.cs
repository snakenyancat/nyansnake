using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

namespace NyanSnake
{
    internal static class Utilities
    {
        private static MethodInfo _getCachedPositionsMethod;

        private static Vector3[] GetSplinePositions(Spline spline)
        {
            if (_getCachedPositionsMethod == null)
            {
                Assembly unitySplinesEditorAssembly = Assembly.Load("Unity.Splines.Editor");
                Type splineCacheUtilityType = unitySplinesEditorAssembly.GetType("UnityEditor.Splines.SplineCacheUtility");
                _getCachedPositionsMethod = splineCacheUtilityType.GetMethod("GetCachedPositions");
            }
            object[] parameters = { spline, null };
            _getCachedPositionsMethod.Invoke(null, parameters);
            return (Vector3[])parameters[1];
        }

        public static void DrawGizmoSpline(Spline spline)
        {
            if (spline != null && spline.Count > 1)
            {
                Vector3[] positions = GetSplinePositions(spline);
                for (int i = 1; i < positions.Length; i++)
                {
                    Gizmos.DrawLine(positions[i - 1], positions[i]);
                }
            }
        }

        public static Rect GetVisibleRect(float margin = 0, bool safeArea = false, float z = 0)
        {
            Vector3 center;
            Vector3 extent;
            if (safeArea)
            {
                Rect screenRect = Screen.safeArea;
                center = Camera.main.ScreenToWorldPoint(screenRect.center);
                center.z = z;
                extent = new Vector2(screenRect.xMax, screenRect.yMax) * (Camera.main.orthographicSize / Screen.height);
            }
            else
            {
                center = Camera.main.transform.position;
                center.z = z;
                extent = new Vector2 (Camera.main.aspect, 1) * Camera.main.orthographicSize;
            }
            extent -= new Vector3(1, 1, 0) * margin;
            return new Rect(center - extent, extent * 2);
        }

        public static Rect GetVisibleRect(RectTransform rectTransform, Vector2 margin = default)
        {
            Canvas.ForceUpdateCanvases();
            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);
            return Rect.MinMaxRect(worldCorners[0].x - margin.x, worldCorners[0].y - margin.y, worldCorners[2].x + margin.x, worldCorners[2].y + margin.y);
        }

        // See https://stackoverflow.com/a/6400477/1785804.
        public static float Modulo(float a, float b)
        {
            return a - b * Mathf.Floor(a / b);
        }

        public static IEnumerator LoadScene(string sceneName, float minTime = 0)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            yield return new WaitForSecondsRealtime(.25f); // Lets the button sound play.
            asyncOperation.allowSceneActivation = true;
        }

        public static void SetGlobalScale(this RectTransform rectTransform, Vector3 globalScale, ref Action revert)
        {
            RectTransform GetParent(RectTransform rectTransform)
            {
                return rectTransform.parent && !rectTransform.parent.GetComponent<Canvas>() ?
                    rectTransform.parent.GetComponent<RectTransform>() :
                    null;
            }
            List<(Transform t, Vector3 s)> scaleSave = new();
            do
            {
                scaleSave.Add((rectTransform, rectTransform.localScale));
                rectTransform.localScale = globalScale;
            }
            while ((rectTransform = GetParent(rectTransform)) != null);
            revert = () => scaleSave.ForEach(_ => _.t.localScale = _.s);
        }
    }
}
