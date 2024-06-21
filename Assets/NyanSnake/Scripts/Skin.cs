using UnityEngine;
using UnityEngine.U2D;

namespace NyanSnake
{
    [CreateAssetMenu(menuName = "Nyan Snake/Skin")]
    internal class Skin : ScriptableObject
    {
        #region Inspector

        [field: SerializeField] public Color UIColor { get; private set; }
        [field: SerializeField] public RuntimeAnimatorController BodyAnimationController { get; private set; }
        [field: SerializeField] public SpriteShape TailSpriteShape { get; private set; }
        [field: SerializeField] public Color StarFieldColor { get; private set; }
        [field: SerializeField] public RuntimeAnimatorController StarAnimationController { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
        [field: SerializeField] public bool Locked { get; private set; }
        [field: SerializeField] public float Price { get; private set; }

        #endregion // Inspector
    }
}
