using UnityEngine;
using static NyanSnake.Sound;

namespace NyanSnake
{
    internal class SoundPlay : MonoBehaviour
    {
        #region Inspector

        [Header("References")]

        [SerializeField] private Effect _effect;

        #endregion // Inspector

        public void Play()
        {
            Sound.Instance.PlayEffect(_effect);
        }
    }
}
