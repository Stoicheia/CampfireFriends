using System;
using UnityEngine;

namespace Minigame
{
    [RequireComponent(typeof(AudioSource))]
    public class RhythmEngine : MonoBehaviour
    {
        private AudioClip _clip;
        private float _bpm;
        private float _offsetSeconds;

        private AudioSource _audioSource;

        public float CurrentTimeSeconds => (float)_audioSource.timeSamples / _clip.frequency;
        public float CurrentTimeBeats => _bpm * CurrentTimeSeconds / 60;
        public float Bpm => _bpm;
        public float OffsetSeconds => _offsetSeconds;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void StartAudio()
        {
            _audioSource.clip = _clip;
            _audioSource.Play();
        }

        public void SetParams(MinigameDefinition definition)
        {
            _clip = definition.Clip;
            _bpm = definition.Bpm;
            _offsetSeconds = definition.OffsetSeconds;
        }
    }
}