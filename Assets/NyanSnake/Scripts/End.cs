using Gists;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NyanSnake
{
    internal class End : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _borderImage;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _replayImage;
        [SerializeField] private Image _quitImage;
        [SerializeField] private TextMeshProUGUI _replayText;
        [SerializeField] private TextMeshProUGUI _quitText;
        [SerializeField] private Button _replayButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Game _game;
        [SerializeField] private Sound _sound;
        [SerializeField] private Curtain _curtain;

        [Header("Settings")]

        [SerializeField] private float _showTime = .5f;
        [SerializeField] private EasingFunction.Ease _showEasing = EasingFunction.Ease.EaseOutBack;

        #endregion // Inspector

        private void Start()
        {
            SetScale(0);
            _replayButton.onClick.AddListener(OnReplayButtonClick);
            _quitButton.onClick.AddListener(OnQuitButtonClick);
            ApplySkin();
            SkinDressing.Instance.OnSkinChanged += ApplySkin;
        }

        private void ApplySkin()
        {
            Skin skin = SkinDressing.Instance.GetSkin();
            _borderImage.color = skin.UIColor;
            _backgroundImage.color = skin.StarFieldColor;
            _text.color = skin.UIColor;
            _replayImage.color = _replayText.color = skin.UIColor;
            _quitImage.color = _quitText.color = skin.UIColor;
        }

        private IEnumerator ShowCoroutine()
        {
            _game.Pause(true);
            _sound.PlayEffect(Sound.Effect.End);
            _sound.PauseMusic(true);
            _curtain.GoDown(true);
            float time = 0;
            while (time < _showTime)
            {
                SetScale(EasingFunction.GetEasingFunction(_showEasing)(0, 1, time / _showTime));
                yield return null;
                time += Time.unscaledDeltaTime;
            }
            SetScale(1);
        }

        private void SetScale(float scale)
        {
            _rectTransform.localScale = Vector3.one * scale;
        }

        private void OnReplayButtonClick()
        {
            _game.Replay();
        }

        private void OnQuitButtonClick()
        {
            _game.Quit();
        }

        public void Show()
        {
            StartCoroutine(ShowCoroutine());
        }
    }
}
