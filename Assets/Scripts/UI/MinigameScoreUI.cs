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

        [SerializeField] private Color _minColor;
        [SerializeField] private Color _maxColor;

        public void Set(float score, float total, Sprite sprt)
        {
            _scoreTextLeft.text = $"{score}";
            _scoreTextRight.text = ((int)total).ToString();
            _scoreTextLeft.color = Color.Lerp(_minColor, _maxColor, score/total);
            _img.sprite = sprt;
        }
    }
}