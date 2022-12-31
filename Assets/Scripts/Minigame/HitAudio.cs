using System;
using System.Collections;
using UnityEngine;

namespace Minigame
{
    [RequireComponent(typeof(AudioSource))]
    public class HitAudio : MonoBehaviour
    {
        private AudioSource _audio;
        [SerializeField] private AudioClip _goodHitsound;
        [SerializeField] private AudioClip _badHitsound;
        [SerializeField] private AudioClip _missSound;

        private bool _ongoingGoodHitsound;
        private bool _ongoingBadHitsound;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            MinigameManager.OnHit += ProcessHit;
            MinigameManager.OnMiss += ProcessMiss;
        }
        
        private void OnDisable()
        {
            MinigameManager.OnHit -= ProcessHit;
            MinigameManager.OnMiss -= ProcessMiss;
        }

        private void ProcessHit(ScanLine scanLine, bool b, HitResult arg3)
        {
            if (b && !_ongoingGoodHitsound)
            {
                _audio.PlayOneShot(_goodHitsound);
                _ongoingGoodHitsound = true;
                StartCoroutine(GoodHitsoundExpireSeq());
            }
            else if (!b && !_ongoingBadHitsound)
            {
                _audio.PlayOneShot(_badHitsound);
                _ongoingBadHitsound = true;
                StartCoroutine(BadHitsoundExpireSeq());
            }
        }

        private void ProcessMiss(ScanLine scanLine, bool b, ScanEvent arg3)
        {
            if (!b) return;
            _audio.volume -= 0.5f;
            _audio.PlayOneShot(_missSound);
            _audio.volume += 0.5f;
        }
        
        IEnumerator GoodHitsoundExpireSeq()
        {
            yield return new WaitForSeconds(_goodHitsound.length/2);
            _ongoingGoodHitsound = false;
        }
        
        IEnumerator BadHitsoundExpireSeq()
        {
            yield return new WaitForSeconds(_badHitsound.length/2);
            _ongoingBadHitsound = false;
        }
        
    }
}