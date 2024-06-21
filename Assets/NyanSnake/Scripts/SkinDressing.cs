using System;
using System.Collections.Generic;
using UnityEngine;

namespace NyanSnake
{
    internal class SkinDressing : MonoBehaviour
    {
        private const string IndexKey = "SkinDressing-Index";

        #region Inspector

        [Header("References")]

        [SerializeField] private List<Skin> _skins;

        #endregion // Inspector

        private static SkinDressing _instance;

        public static SkinDressing Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindAnyObjectByType<SkinDressing>();
                    _instance.Init();
                }
                return _instance;
            }
        }

        private int _index;

        public event Action OnSkinChanged;

        private void Init()
        {
            if (!TryRestoreIndex(out _index))
            {
                SaveIndex(_index = 0);
            }
        }

        private bool TryRestoreIndex(out int index)
        {
            index = PlayerPrefs.GetInt(IndexKey);
            return PlayerPrefs.HasKey(IndexKey);
        }

        private void SaveIndex(int index)
        {
            PlayerPrefs.SetInt(IndexKey, index);
        }

        [ContextMenu("Next Skin")]
        public void NextSkin()
        {
            SaveIndex(_index = (int)Utilities.Modulo(_index + 1, _skins.Count));
            OnSkinChanged?.Invoke();
        }

        [ContextMenu("Previous Skin")]
        public void PreviousSkin()
        {
            SaveIndex(_index = (int)Utilities.Modulo(_index - 1, _skins.Count));
            OnSkinChanged?.Invoke();
        }

        public Skin GetSkin() => _skins[_index];
    }
}
