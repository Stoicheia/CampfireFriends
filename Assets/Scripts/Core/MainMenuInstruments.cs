using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class MainMenuInstruments : MonoBehaviour
    {
        [SerializeField] private List<AudioSource> _instruments;
        [SerializeField] private MainGameManager _main;

        private void OnEnable()
        {
            int current = _main.CurrentStep;
            for (int i = 0; i <= current; i++)
            {
                _instruments[i].Play();
            }
        }

        private void OnDisable()
        {
            foreach (var s in _instruments)
            {
                s.Stop();
            }
        }
    }
}