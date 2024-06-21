using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static NyanSnake.Sound;

namespace NyanSnake
{
    internal class Tutorial : MonoBehaviour
    {
        private const string ShownKey = "Tutorial-Shown";

        #region Inspector

        [Header("References")]

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Button _button;
        [SerializeField] private Game _game;
        [SerializeField] private Sound _sound;
        [SerializeField] private Curtain _curtain;
        [SerializeField] private RectTransform _upRectTransform;
        [SerializeField] private RectTransform _downRectTransform;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private SpriteRenderer _bodySpriteRenderer;
        [SerializeField] private Animator _bodyAnimator;

        [Header("Settings")]

        [SerializeField] private float _musicTransitionTime = .5f;

        #endregion // Inspector

        public void Show()
        {
            StartCoroutine(ShowCoroutine());
        }

        public IEnumerator ShowCoroutine()
        {
            bool shown = GetShown();
            if (!shown)
            {
                SaveShown(shown = true);
                _rectTransform.localScale = Vector3.one;
                bool clicked = false;
                _button.onClick.AddListener(() => clicked = true);
                _bodyAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
                int bodySortingOrder = _bodySpriteRenderer.sortingOrder;
                _bodySpriteRenderer.sortingOrder = _canvas.sortingOrder + 1;
                _game.Pause(true);
                _sound.PlayMusic(Music.Tutorial, _musicTransitionTime);
                _curtain.GoDown(true);
                _upRectTransform.position = _bodyTransform.position + new Vector3(0, 1, 0);
                _downRectTransform.position = _bodyTransform.position + new Vector3(0, -1, 0);
                yield return new WaitUntil(() => clicked);
                _game.Pause(false);
                _sound.PlayMusic(Music.Game, _musicTransitionTime);
                _curtain.GoDown(false);
                _bodySpriteRenderer.sortingOrder = bodySortingOrder;
                _bodyAnimator.updateMode = AnimatorUpdateMode.Normal;
                _rectTransform.localScale = Vector3.zero;
            }
        }

        private bool TryRestoreShown(out bool shown)
        {
            shown = (PlayerPrefs.GetInt(ShownKey) == 1);
            return PlayerPrefs.HasKey(ShownKey);
        }

        private void SaveShown(bool shown)
        {
            PlayerPrefs.SetInt(ShownKey, shown ? 1 : 0);
        }

        public bool GetShown()
        {
            bool shown;
            if (!TryRestoreShown(out shown))
            {
                SaveShown(shown);
            }
            return shown;
        }
    }
}
