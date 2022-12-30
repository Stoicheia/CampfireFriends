using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MinigameScoreUI : MonoBehaviour
    {
        [SerializeField] private Image _img;
        [SerializeField] private TextMeshProUGUI _scoreTextLeft;
        [SerializeField] private TextMeshProUGUI _scoreTextRight;

        public void Set(float score, float total, Sprite sprt)
        {
            _scoreTextLeft.text = $"{score}";
            _scoreTextRight.text = ((int)total).ToString();
            _img.sprite = sprt;
        }
    }
}