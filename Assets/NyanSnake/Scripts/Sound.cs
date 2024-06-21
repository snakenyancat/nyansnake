using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace NyanSnake
{
    internal class Sound : MonoBehaviour
    {
        [Serializable]
        private class EffectInfo
        {
            [field: SerializeField] public Effect Effect { get; private set; }
            [field: SerializeField] public AudioClip AudioClip { get; private set; }
            [field: SerializeField] public AudioMixerGroup AudioMixerGroup { get; private set; }
        }

        public enum Effect
        {
            End,
            Eat,
            Button,
            MainButton,
            Turn
        }

        [Serializable]
        private class MusicInfo
        {
            [field: SerializeField] public Music Music { get; private set; }
            [field: SerializeField] public float StartTime { get; private set; }
            [field: SerializeField] public AudioMixerSnapshot AudioMixerSnapshot { get; private set; }
        }

        public enum Music
        {
            Menu,
            Tutorial,
            Game
        }

        #region Inspector

        [Header("References")]

        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _effectAudioSource;
        [SerializeField, FormerlySerializedAs("_effectAudioClips")] private List<EffectInfo> _effectInfos;
        [SerializeField] private List<MusicInfo> _musicInfos;
        [SerializeField] private Music _music;

        #endregion // Inspector

        private static Sound _instance;

        public static Sound Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindAnyObjectByType<Sound>();
                }
                return _instance;
            }
        }

        private float _musicTime;
        private Dictionary<Effect, EffectInfo> _effectInfosAsMap;
        private Dictionary<Music, MusicInfo> _musicInfosAsMap;

        private void Start()
        {
            _effectInfosAsMap = _effectInfos.ToDictionary(effectInfo => effectInfo.Effect, effectInfo => effectInfo);
            _musicInfosAsMap = _musicInfos.ToDictionary(musicInfo => musicInfo.Music, musicInfo => musicInfo);
            _musicTime = Time.time - _musicInfosAsMap[_music].StartTime;
            ApplySkin();
            SkinDressing.Instance.OnSkinChanged += ApplySkin;
        }

        private void ApplySkin()
        {
            Skin skin = SkinDressing.Instance.GetSkin();
            _musicAudioSource.clip = skin.AudioClip;
            _musicAudioSource.time = (Time.time - _musicTime) % _musicAudioSource.clip.length;
            _musicInfosAsMap[_music].AudioMixerSnapshot.TransitionTo(0);
            _musicAudioSource.Play();
        }

        public void PlayMusic(Music music, float transitionTime = 1)
        {
            _music = music;
            _musicInfosAsMap[_music].AudioMixerSnapshot.TransitionTo(transitionTime);
        }

        public void PauseMusic(bool on)
        {
            if (on)
            {
                _musicAudioSource.Pause();
            }
            else
            {
                _musicAudioSource.Play();
            }
        }

        public void PlayEffect(Effect effect)
        {
            EffectInfo effectInfo = _effectInfosAsMap[effect];
            _effectAudioSource.clip = effectInfo.AudioClip;
            _effectAudioSource.outputAudioMixerGroup = effectInfo.AudioMixerGroup;
            _effectAudioSource.Play();
        }
    }
}
