using System;
using Minigame;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(DialogeMan))]
    public class MinigameGiver : MonoBehaviour
    {
        [SerializeField] private MinigameDefinition _myMinigame;
        private DialogeMan _dialoge;

        public MinigameDefinition Config => _myMinigame;

        private void Awake()
        {
            _dialoge = GetComponent<DialogeMan>();
        }
    }
}