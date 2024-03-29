﻿using UnityEngine;

namespace UI
{
    public class CountdownUI : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void InitCountdown(float bpm)
        {
            gameObject.SetActive(true);
            _animator.speed = bpm / 60;
            _animator.Play("countdown", 0);
        }
    }
}