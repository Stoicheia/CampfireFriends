using System;
using UnityEngine;

namespace Minigame
{
    public class ComboSystem : MonoBehaviour
    {
        public event Action<int> OnComboIncrease;
        public event Action OnComboEnd;
        public int Combo { get; set; }
        private void OnEnable()
        {
            Combo = 0;
            OnComboIncrease?.Invoke(0);
            MinigameManager.OnHit += HandleHit;
            MinigameManager.OnMiss += HandleMiss;
        }

        private void OnDisable()
        {
            MinigameManager.OnHit -= HandleHit;
            MinigameManager.OnMiss -= HandleMiss;
        }

        private void HandleHit(ScanLine scanLine, bool b, HitResult arg3)
        {
            if (b)
            {
                Combo++;
                OnComboIncrease?.Invoke(Combo);
            }
            else
            {
                Combo = 0;
                OnComboEnd?.Invoke();
            }
        }

        private void HandleMiss(ScanLine scanLine, bool b, ScanEvent arg3)
        {
            if (b)
            {
                Combo = 0;
                OnComboEnd?.Invoke();
            }
        }
    }
}