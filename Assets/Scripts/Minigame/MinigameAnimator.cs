﻿using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Minigame
{
    public class MinigameAnimator : MonoBehaviour
    {
        [SerializeField] private CountdownUI _countdown;
        [SerializeField] private Animator _collectThese;
        [SerializeField] private ResultsScreenUI _resultsScreen;
        [SerializeField] private MinigameManager _game;

        private void Start()
        {
            _game.OnEnd += ShowEndScreen;
        }

        private void OnDisable()
        {
            ResetAll();
        }

        public void Begin()
        {
            StartCoroutine(StartSequence());
        }

        private IEnumerator StartSequence()
        {
            float wait = 4.5f * 60 / _game.Bpm;
            _game.enabled = true;
            _game.StartGame(2 + wait);
            _collectThese.Play("collectThese");
            yield return new WaitForSeconds(2);
            _countdown.InitCountdown(_game.Bpm);
            yield return null;
            yield return new WaitForSeconds(wait);
            _countdown.gameObject.SetActive(false);
        }

        private void ShowEndScreen(MinigameResults minigameResults)
        {
            _resultsScreen.gameObject.SetActive(true);
            _resultsScreen.Display(minigameResults);
        }

        private void ResetAll()
        {
            _resultsScreen.gameObject.SetActive(false);
        }
    }
}