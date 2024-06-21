using UnityEngine;
using TMPro;

namespace NyanSnake
{
    internal class ScoreMenu : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private TextMeshProUGUI _text;

        #endregion // Inspector

        private void Start()
        {
            int score = ScoreRecord.Instance.GetBestScore();
            _text.text = $"Best score: {score}";
            ApplySkin();
            SkinDressing.Instance.OnSkinChanged += ApplySkin;
        }

        private void ApplySkin()
        {
            Skin skin = SkinDressing.Instance.GetSkin();
            _text.color = skin.UIColor;
        }
    }
}
