using System;
using System.Collections.Generic;
using Core;
using Minigame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResultsScreenUI : MonoBehaviour
    {
        private MinigameResults _results;
        [SerializeField] private List<Image> _goodSprites;
        [SerializeField] private List<TextMeshProUGUI> _goodTexts;
        [SerializeField] private TextMeshProUGUI _badText;
        [SerializeField] private TextMeshProUGUI _accuracyText;

        [SerializeField] private Animator _animator;

        public void Display(MinigameResults results)
        {
            _results = results;
            _animator.Play("slideIn", 0);

            var goodScores = results.GoodItemScores;
            var goodTotals = results.GoodItemTotals;
            int i = 0;
            foreach(var gi in results.GoodItems)
            {
                _goodSprites[i].sprite = gi.Sprite;
                _goodTexts[i].text = $"{goodScores[gi]}/{goodTotals[gi]}";
                i++;
            }

            _badText.text = $"{results.BadItemScore}/{results.BadItemTotal}";
            _accuracyText.text = $"{(int)results.CalculateAccuracyPercent()}%";
        }

        public void BackToMenu()
        {
            SceneCameraNavigator.Instance.TransitionToMain();
        }
    }
}