using System;
using System.Collections.Generic;
using Minigame;
using UI;
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

        private void OnEnable()
        {
            if(_ptr < _characters.Count)
                StartCharacterInitialDialogue(_characters[_ptr]);
            else
            {
                foreach (var c in _characters)
                {
                    StartCharacterFinalDialogue(c);
                }
            }
        }

        private void OnDisable()
        {
            _ptr++;
        }

        private void Start()
        {
            foreach (var a in _characters)
            {
                a.MyManager = this;
            }
            _data = PlayerData.Instance;
        }

        public void StartCharacterInitialDialogue(DialogeMan man)
        {
            man.RunInitial();
        }
        
        public void StartCharacterFinalDialogue(DialogeMan man)
        {
            man.RunFinal();
        }


        public void ShowStartButton(MinigameDefinition config)
        {
            _startButton.Play("buttonUp");
            var button = _startButton.GetComponent<MinigameLaunchButton>();
            button.ToLaunch = config;
        }
    }
}