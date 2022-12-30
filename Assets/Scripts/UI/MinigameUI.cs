using System;
using System.Collections.Generic;
using Minigame;
using UnityEngine;

namespace UI
{
    public class MinigameUI : MonoBehaviour
    {
        [SerializeField] private MinigameManager _game;
        [Space] [SerializeField] private List<MinigameScoreUI> _scoreUIs;
        [SerializeField] private MinigameScoreUISimple _junkScoreUI;
        private Dictionary<PrimitiveItem, MinigameScoreUI> _itemToScoreUI;
        [SerializeField] private ProgressBarUI _progressUI;

        private bool _started = false;

        private void OnEnable()
        {
            _game.OnInit += Init;
            _started = false;
        }

        private void OnDisable()
        {
            _game.OnInit -= Init;
        }

        private void Awake()
        {
            _itemToScoreUI = new Dictionary<PrimitiveItem, MinigameScoreUI>();
        }

        private void Update()
        {
            if (!_started) return;
            foreach (var ui in _itemToScoreUI)
            {
                PrimitiveItem item = ui.Key;
                ui.Value.Set(_game.GetScore(item), _game.GetMaxScore(item), ui.Key.Sprite);
            }
            _junkScoreUI.Set(_game.SumOfBadScores);
            _progressUI.SetProgress(_game.GetSongProgress());
        }

        private void Init()
        {
            _itemToScoreUI = new Dictionary<PrimitiveItem, MinigameScoreUI>();
            for (int i = 0; i < Math.Min(_scoreUIs.Count, _game.GoodItems.Count); i++)
            {
                _itemToScoreUI.Add(_game.GoodItems[i], _scoreUIs[i]);
                _scoreUIs[i].gameObject.SetActive(true);
            }
            
            for (int i = Math.Min(_scoreUIs.Count, _game.GoodItems.Count); i < _scoreUIs.Count; i++)
            {
                _scoreUIs[i].gameObject.SetActive(false);
            }

            _started = true;
        }
    }
}