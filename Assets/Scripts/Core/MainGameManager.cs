using System;
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
        [SerializeField] private Animator _endButton;
        [SerializeField] private Animator _cameraAnim;
        private int _ptr;

        public int CurrentStep => _ptr;

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
            int k = 0;
            for (int i = _characters.Count - 1; i >= 0; i--)
            {
                var m = _characters[i];
                if (m.Equals(man))
                {
                    m.StandingSprite.gameObject.SetActive(true);
                    m.SittingSprite.gameObject.SetActive(false);
                    m.TextBubble.gameObject.SetActive(true);
                    k = i;
                    break;
                }
                else
                {
                    m.StandingSprite.gameObject.SetActive(false);
                    m.SittingSprite.gameObject.SetActive(false);
                    m.TextBubble.gameObject.SetActive(false);
                }
            }

            for (int i = k - 1; i >= 0; i--)
            {
                var m = _characters[i];
                m.StandingSprite.gameObject.SetActive(false);
                m.SittingSprite.gameObject.SetActive(true);
                m.TextBubble.gameObject.SetActive(false);
            }
            
           
            man.RunInitial();
        }
        
        public void StartCharacterFinalDialogue(DialogeMan man)
        {
            foreach (var m in _characters)
            {
                m.StandingSprite.gameObject.SetActive(false);
                m.SittingSprite.gameObject.SetActive(true);
                m.TextBubble.gameObject.SetActive(true);
            }
            man.RunFinal();
        }


        public void ShowStartButton(MinigameDefinition config)
        {
            _startButton.gameObject.SetActive(true);
            _startButton.Play("buttonUp");
            var button = _startButton.GetComponent<MinigameLaunchButton>();
            button.ToLaunch = config;
        }
        
        public void ShowEndButton()
        {
            _endButton.gameObject.SetActive(true);
            _endButton.Play("buttonUp");
        }

        IEnumerator StartSequence()
        {
            if (_ptr == 0)
            {
                foreach (var m in _characters)
                {
                    m.StandingSprite.gameObject.SetActive(false);
                    m.SittingSprite.gameObject.SetActive(false);
                    m.TextBubble.gameObject.SetActive(false);
                }
                _cameraAnim.Play("intro");
                yield return new WaitForSeconds(2f);
            }

            else
            {
                _cameraAnim.Play("idle");
            }

            _startButton.gameObject.SetActive(false);
            Animator a = _characters[_ptr].GetComponent<Animator>();
            if (a != null)
            {
                a.Play("enter");
                yield return new WaitForSeconds(1.75f);
            }

            foreach (var c in _characters)
            {
                Animator ca = c.GetComponent<Animator>();
                if (ca == null) continue;
                if (!ca.Equals(a))
                {
                    ca.Play("sit");
                }
            }
            yield return new WaitForSeconds(0.3f);
            NextDialogue();
        }
    }
}