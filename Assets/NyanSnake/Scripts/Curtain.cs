using Gists;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NyanSnake
{
    internal class Curtain : MonoBehaviour
    {
        private delegate IEnumerator Coroutine(Action onDone);

        #region Inspector

        [Header("References")]

        [SerializeField] private Image _image;

        [Header("Settings")]

        [SerializeField, Range(0, 1)] private float _alphaMax = .2f;
        [SerializeField]              private float _upTime = .25f;
        [SerializeField]              private float _downTime = .25f;
        [SerializeField]              private EasingFunction.Ease _upEasing = EasingFunction.Ease.EaseOutQuint;
        [SerializeField]              private EasingFunction.Ease _downEasing = EasingFunction.Ease.EaseOutQuint;

        #endregion // Inspector

        private Queue<Coroutine> _coroutines;
        private bool _isCoroutineExecuting;

        private void Start()
        {
            SetAlpha(0);
            _coroutines = new Queue<Coroutine>();
            _isCoroutineExecuting = false;
        }

        private void Update()
        {
            if (_coroutines.Count > 0 && !_isCoroutineExecuting)
            {
                _isCoroutineExecuting = true;
                StartCoroutine(_coroutines.Dequeue()(() => _isCoroutineExecuting = false));
            }
        }

        private IEnumerator GoDownCoroutine(Action onDone)
        {
            float time = 0;
            while (time < _downTime)
            {
                SetAlpha(EasingFunction.GetEasingFunction(_downEasing)(0, _alphaMax, time / _downTime));
                yield return null;
                time += Time.unscaledDeltaTime;
            }
            SetAlpha(_alphaMax);
            onDone();
        }

        private IEnumerator GoUpCoroutine(Action onDone)
        {
            float time = 0;
            while (time < _upTime)
            {
                SetAlpha(EasingFunction.GetEasingFunction(_upEasing)(_alphaMax, 0, time / _upTime));
                yield return null;
                time += Time.unscaledDeltaTime;
            }
            SetAlpha(0);
            onDone();
        }

        private void SetAlpha(float alpha)
        {
            Color imageColor = _image.color;
            imageColor.a = alpha;
            _image.color = imageColor;
        }

        public void GoDown(bool down)
        {
            _coroutines.Enqueue(down ? GoDownCoroutine : GoUpCoroutine);
        }
    }
}
