using System;
using System.Collections;
using UnityEngine;

namespace Minigame
{
    [RequireComponent(typeof(AudioSource))]
    public class RhythmEngine : MonoBehaviour
    {
        public event Action OnSongEnd;
        
        private AudioClip _clip;
        private float _bpm;
        private float _offsetSeconds;

        private AudioSource _audioSource;
        private bool _hasStarted;
        private bool _inCountdown;
        private float _countdownTimer;

        public float CurrentTimeSeconds => _inCountdown ? _countdownTimer : (float)_audioSource.timeSamples / _clip.frequency;
        public float TotalTimeSeconds => _clip.length;
        public float CurrentTimeBeats => _bpm * CurrentTimeSeconds / 60;
        public float Bpm => _bpm;
        public float OffsetSeconds => _offsetSeconds;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_hasStarted && !_audioSource.isPlaying)
            {
                _hasStarted = false;
                OnSongEnd?.Invoke();
            }
        }

        public void StartAudio(float countdown = 0)
        {
            StartCoroutine(Countdown(countdown));
        }

        public void SetParams(MinigameDefinition definition)
        {
            _clip = definition.Clip;
            _bpm = definition.Bpm;
            _offsetSeconds = definition.OffsetSeconds;
        }

        private IEnumerator Countdown(float t)
        {
            _countdownTimer = -t;
            _inCountdown = true;
            while (_countdownTimer < 0)
            {
                _countdownTimer += Time.deltaTime;
                yield return null;
            }
            _audioSource.clip = _clip;
            _audioSource.Play();
            _hasStarted = true;
            _inCountdown = false;
        }
    }
}