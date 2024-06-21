using UnityEngine;

namespace NyanSnake
{
    internal class Star : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private Animator _animator;

        #endregion // Inspector

        public SkinDressing SkinDressing { get; set; }

        private void Start()
        {
            ApplySkin();
            SkinDressing.Instance.OnSkinChanged += ApplySkin;
        }

        private void ApplySkin()
        {
            Skin skin = SkinDressing.Instance.GetSkin();
            _animator.runtimeAnimatorController = skin.StarAnimationController;
            _animator.SetFloat("CycleOffset", Random.value);
        }
    }
}
