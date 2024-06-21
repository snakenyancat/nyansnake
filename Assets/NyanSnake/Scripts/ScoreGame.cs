using UnityEngine;
using TMPro;

namespace NyanSnake
{
    internal class ScoreGame : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private TextMeshProUGUI _text;

        #endregion // Inspector

        private int _score;

        private void Start()
        {
            SetScore(0);
            ApplySkin();
            SkinDressing.Instance.OnSkinChanged += ApplySkin;
        }

        private void ApplySkin()
        {
            Skin skin = SkinDressing.Instance.GetSkin();
            _text.color = skin.UIColor;
        }

        public void SetScore(int score)
        {
            _score = score;
            _text.text = $"Score: {score}";
        }

        public int GetScore()
        {
            return _score;
        }
    }
}
