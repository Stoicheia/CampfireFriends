﻿using System;
using System.Collections;
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
            StartCoroutine(StartSequence());
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

        public void NextDialogue()
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
            _startButton.gameObject.SetActive(true);
            _startButton.Play("buttonUp");
            var button = _startButton.GetComponent<MinigameLaunchButton>();
            button.ToLaunch = config;
        }

        IEnumerator StartSequence()
        {
            _startButton.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            NextDialogue();
        }
    }
}