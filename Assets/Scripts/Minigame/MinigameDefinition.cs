using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigame
{
    [CreateAssetMenu(fileName = "Minigame Config", menuName = "Minigame Config", order = 0)]
    public class MinigameDefinition : ScriptableObject
    {
        [Serializable]
        public class ItemQuantityPair
        {
            public ItemData Item;
            public int Quantity;
        }
        
        public AudioClip Clip => _clip;
        public float Bpm => _bpm;
        public float OffsetSeconds => _offsetSeconds;
        public List<ItemData> AllItems => _allPossibleItems;
        public int TotalBeatCount => (int)Math.Floor(_bpm * _clip.length / 60);
        public int Subdivisions => _subdivisions;
        public float LeniencySeconds => _leniencyBeats * 60 / _bpm;
        public float LeniencyPerfectSeconds => _perfectExactness * _leniencyBeats * 60 / _bpm;
        public float ApproachSeconds => _approachSeconds;
        public float GoodItemDensity => _goodItemDensity;
        public float GoodItemVariance => _goodItemVariance;
        public List<ItemData> GoodItems => _goodItemList;

        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _bpm;
        [SerializeField] private float _offsetSeconds;
        [SerializeField] private List<ItemData> _goodItemList;
        [SerializeField] private List<ItemData> _allPossibleItems;
        [SerializeField] [Range(0, 0.9f)] private float _goodItemDensity;
        [SerializeField] [Range(0, 0.1f)] private float _goodItemVariance;
        [SerializeField] private int _subdivisions;
        [Space] [SerializeField] private float _leniencyBeats;
        [SerializeField][Range(0, 1)] private float _perfectExactness;
        [SerializeField] private float _approachSeconds;
        
        public ItemData GetRandomItem()
        {
            ItemData item = _allPossibleItems[Random.Range(0, _allPossibleItems.Count)];
            return item;
        }

        public ItemData GetRandomBadItem()
        {
            ItemData item = null;
            List<ItemData> disallowed = _goodItemList;

            while (disallowed.Contains(item) || item == null)
            {
                item = _allPossibleItems[Random.Range(0, _allPossibleItems.Count)];
            }
            
            return item;
        }
    }
}