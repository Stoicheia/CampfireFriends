using System.Collections.Generic;
using Minigame;
using UnityEngine;

namespace Core
{
    public class PlayerData : MonoBehaviour
    {
        public static PlayerData Instance;

        private Dictionary<AnimalType, MinigameResults> _animalToResults;

        public float GetAccuracy(AnimalType t) => _animalToResults[t].CalculateAccuracyPercent();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            _animalToResults = new Dictionary<AnimalType, MinigameResults>();
        }

        public void AddResult(AnimalType a, MinigameResults r)
        {
            _animalToResults.Add(a, r);
        }
    }
}