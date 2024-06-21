using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace NyanSnake
{
    internal class SkinChoice : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private Button _nextSkinButton;
        [SerializeField] private Button _previousSkinButton;
        [SerializeField] private SpriteRenderer _bodySpriteRenderer;
        [SerializeField] private Animator _bodyAnimator;
        [SerializeField] private SpriteShapeRenderer _tailSpriteShapeRenderer;
        [SerializeField] private SpriteShapeController _tailSpriteShapeController;
        [SerializeField] private Image _lockImage;

        #endregion // Inspector

        private void Start()
        {
            _nextSkinButton.onClick.AddListener(OnNextSkinButtonClick);
            _previousSkinButton.onClick.AddListener(OnPreviousSkinButtonClick);
            ApplySkin();
            SkinDressing.Instance.OnSkinChanged += ApplySkin;
        }

        private void ApplySkin()
        {
            Skin skin = SkinDressing.Instance.GetSkin();
            _nextSkinButton.targetGraphic.color = _previousSkinButton.targetGraphic.color = skin.UIColor;
            _bodySpriteRenderer.color = _tailSpriteShapeRenderer.color = (skin.Locked ? Color.grey : Color.white);
            _bodyAnimator.runtimeAnimatorController = skin.BodyAnimationController;
            _tailSpriteShapeController.spriteShape = skin.TailSpriteShape;
            Color lockImageColor = skin.UIColor;
            lockImageColor.a = (skin.Locked ? 1 : 0);
            _lockImage.color = lockImageColor;
        }

        private void OnNextSkinButtonClick()
        {
            SkinDressing.Instance.NextSkin();
        }

        private void OnPreviousSkinButtonClick()
        {
            SkinDressing.Instance.PreviousSkin();
        }
    }
}
