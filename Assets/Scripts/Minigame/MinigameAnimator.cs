using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Minigame
{
    public class MinigameAnimator : MonoBehaviour
    {
        [SerializeField] private CountdownUI _countdown;
        [SerializeField] private ResultsScreenUI _resultsScreen;
        [SerializeField] private MinigameManager _game;

        private void Start()
        {
            _game.OnEnd += ShowEndScreen;
        }

        public void Begin()
        {
            StartCoroutine(StartSequence());
        }

        private IEnumerator StartSequence()
        {
            _game.enabled = true;
            _countdown.InitCountdown(_game.Bpm);
            float wait = 4.5f * 60 / _game.Bpm;
            _game.StartGame(wait);
            yield return new WaitForSeconds(wait);
            _countdown.gameObject.SetActive(false);
        }

        private void ShowEndScreen(MinigameResults minigameResults)
        {
            _resultsScreen.gameObject.SetActive(true);
            _resultsScreen.Display(minigameResults);
        }
    }
}