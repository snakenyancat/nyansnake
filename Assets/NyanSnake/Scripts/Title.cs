using TMPro;
using UnityEngine;

namespace NyanSnake
{
    internal class Title : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private TextMeshProUGUI _text;

        #endregion // Inspector

        private void Start()
        {
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
