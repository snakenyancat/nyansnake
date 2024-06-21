using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NyanSnake
{
    internal class Play : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private Image _buttonImage;

        [Header("Settings")]

        [SerializeField] private string _gameSceneName;

        #endregion // Inspector

        private void Start()
        {
            ApplySkin();
            SkinDressing.Instance.OnSkinChanged += ApplySkin;
        }

        private void ApplySkin()
        {
            Skin skin = SkinDressing.Instance.GetSkin();
            _buttonText.color = _buttonImage.color = skin.UIColor;
            _buttonText.text = (skin.Locked ? $"Unlock for {string.Format(new CultureInfo("en-US"), "{0:C}", skin.Price)}!" : "Play game!");
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(skin.Locked ? () => BuySkin(skin) : PlayGame);
        }

        private void PlayGame()
        {
            StartCoroutine(Utilities.LoadScene(_gameSceneName, minTime: .1f));
        }

        private void BuySkin(Skin skin)
        {
            // TODO: Implement.
        }
    }
}
