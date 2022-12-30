using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class MainGameManager : MonoBehaviour
    {
        private PlayerData _data;
        [SerializeField] private List<DialogeMan> _characters;
        [SerializeField] private Animator _startButton;
        private int _ptr;

        private void Awake()
        {
            _ptr = 0;
        }

        private void Start()
        {
            foreach (var a in _characters)
            {
                a.MyManager = this;
            }
            _data = PlayerData.Instance;
            StartCharacterInitialDialogue(_characters[0]);
        }

        public void StartCharacterInitialDialogue(DialogeMan man)
        {
            man.RunInitial();
        }

        public void ShowStartButton()
        {
            _startButton.Play("buttonUp");
        }
    }
}