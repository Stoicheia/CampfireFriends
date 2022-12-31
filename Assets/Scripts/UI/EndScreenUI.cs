using System;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class EndScreenUI : MonoBehaviour
    {
        [SerializeField] private List<string> _reloadScenes;
        [SerializeField] private List<TextMeshProUGUI> _accTexts;

        private void Start()
        {
            DisplayAccuracies();
        }

        public void DisplayAccuracies()
        {
            _accTexts[0].text = $"{((int)PlayerData.Instance.GetAccuracy(AnimalType.Bear)).ToString()}%";
            _accTexts[1].text = $"{((int)PlayerData.Instance.GetAccuracy(AnimalType.Squirrel)).ToString()}%";
            _accTexts[2].text = $"{((int)PlayerData.Instance.GetAccuracy(AnimalType.Fox)).ToString()}%";
        }

        public void RestartGame()
        {
            SceneManager.UnloadSceneAsync("EndScreen");
            foreach (var s in _reloadScenes)
            {
                SceneManager.LoadScene(s, LoadSceneMode.Additive);
            }
            
        }
    }
}