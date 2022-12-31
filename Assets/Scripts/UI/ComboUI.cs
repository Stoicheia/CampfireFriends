using System;
using Minigame;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ComboUI : MonoBehaviour
    {
        [SerializeField] private ComboSystem _combo;
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private Animator _animator;

        private void OnEnable()
        {
            _combo.OnComboIncrease += IncreaseCombo;
            _combo.OnComboEnd += EndCombo;
            _comboText.text = "";
        }
        
        private void OnDisable()
        {
            _combo.OnComboIncrease -= IncreaseCombo;
            _combo.OnComboEnd -= EndCombo;
        }

        private void IncreaseCombo(int c)
        {
            _comboText.text = c > 2 ? c.ToString() : "";
            _animator.Play("comboUp");
        }

        private void EndCombo()
        {
            _comboText.text = "";
            _animator.Play("comboDown");
        }
    }
}