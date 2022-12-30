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
            public PrimitiveItem Item;
            public int Quantity;
        }
        
        public AudioClip Clip => _clip;
        public float Bpm => _bpm;
        public float OffsetSeconds => _offsetSeconds;
        public List<ItemQuantityPair> GoodItems => _goodItems;
        public int TotalBeatCount => (int)Math.Floor(_bpm * _clip.length / 60);

        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _bpm;
        [SerializeField] private float _offsetSeconds;
        [SerializeField] private List<ItemQuantityPair> _goodItems;
        [SerializeField] private List<PrimitiveItem> _allPossibleItems;
        
        public PrimitiveItem GetRandomItem()
        {
            PrimitiveItem item = _allPossibleItems[Random.Range(0, _allPossibleItems.Count)];
            return item;
        }

        public PrimitiveItem GetRandomBadItem()
        {
            PrimitiveItem item = _goodItems[0].Item;
            List<PrimitiveItem> disallowed = _goodItems.Select(x => x.Item).ToList();

            while (disallowed.Contains(item))
            {
                item = _allPossibleItems[Random.Range(0, _allPossibleItems.Count)];
            }
            
            return item;
        }
    }
}