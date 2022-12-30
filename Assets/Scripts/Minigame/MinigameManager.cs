using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace Minigame
{
    public class MinigameManager : MonoBehaviour
    {
        public event Action OnInit;
        
        [SerializeField] private MinigameDefinition _minigameConfig;
        [SerializeField] private List<ScanLine> _scanLines;
        [SerializeField] private RhythmEngine _rhythmEngine;

        private List<PrimitiveItem> _itemsToSpawn;
        private bool _started;

        private Dictionary<PrimitiveItem, float> _itemsCollected;

        public List<PrimitiveItem> GoodItems => _minigameConfig.GoodItems.Select(x => x.Item).ToList();

        public float SumOfBadScores => _itemsCollected
            .Where(x => !GoodItems.Contains(x.Key))
            .Select(x => x.Value)
            .Aggregate((x, y) => x + y);

        private void OnEnable()
        {
            foreach (var line in _scanLines)
            {
                line.OnHit += ProcessHit;
            }

            _itemsCollected = new Dictionary<PrimitiveItem, float>();
            ResetAllLines();
            GenerateItemsToSpawn(_scanLines.Count * _minigameConfig.TotalBeatCount / _minigameConfig.Subdivisions);
            DistributeToLines();
            _started = false;
        }

        private void OnDisable()
        {
            foreach (var line in _scanLines)
            {
                line.OnHit -= ProcessHit;
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space) && !_started)
            {
                StartGame();
                _started = true;
            }
        }

        public void StartGame()
        {
            foreach (var t in _scanLines)
            {
                t.Init();
            }

            foreach (var item in _minigameConfig.AllItems)
            {
                _itemsCollected[item] = 0;
            }
            _rhythmEngine.SetParams(_minigameConfig);
            _rhythmEngine.StartAudio();
            
            OnInit?.Invoke();
        }

        public float GetScore(PrimitiveItem item)
        {
            return _itemsCollected[item];
        }
        
        public float GetMaxScore(PrimitiveItem item)
        {
            var goodItems = _minigameConfig.GoodItems;
            foreach (var gi in goodItems)
            {
                if (gi.Item.Equals(item))
                {
                    return gi.Quantity;
                }
            }

            return -1;
        }

        public float GetSongProgress()
        {
            return _rhythmEngine.CurrentTimeSeconds / _rhythmEngine.TotalTimeSeconds;
        }

        private void ProcessHit(ScanEvent @event, float t)
        {
            float error = t - @event.TimeSeconds;
            Debug.Log($"{@event.RequestedObject.Name} collected with error {error}");

            HitResult result;
            if (Mathf.Abs(error) <= _minigameConfig.LeniencyPerfectSeconds) result = HitResult.Perfect;
            else if (Mathf.Abs(error) <= _minigameConfig.LeniencySeconds) result = HitResult.Hit;
            else result = HitResult.Miss;

            float score = result switch
            {
                HitResult.Perfect => 1,
                HitResult.Hit => 0.5f,
                HitResult.Miss => 0
            };

            _itemsCollected[@event.RequestedObject] += score;
            Debug.Log($"{@event.RequestedObject.Name} score = {_itemsCollected[@event.RequestedObject]}");
        }

        private void GenerateItemsToSpawn(int totalNumber)
        {
            List<PrimitiveItem> toSpawn = new List<PrimitiveItem>();
            foreach (var pair in _minigameConfig.GoodItems)
            {
                for (int i = 0; i < pair.Quantity; i++)
                {
                    toSpawn.Add(pair.Item);
                }
            }

            while (toSpawn.Count < totalNumber)
            {
                toSpawn.Add(_minigameConfig.GetRandomItem());
            }
            
            Utility.Shuffle(toSpawn);
            _itemsToSpawn = toSpawn;
        }

        private void ResetAllLines()
        {
            foreach (var l in _scanLines)
            {
                l.Reset();
            }
        }

        private void DistributeToLines()
        {
            for (int i = 0; i < _itemsToSpawn.Count; i++)
            {
                float time = _minigameConfig.OffsetSeconds + _minigameConfig.Subdivisions * _minigameConfig.Bpm * (i/4) / 60;
                _scanLines[i % _scanLines.Count].AddEvent(new ScanEvent(_itemsToSpawn[i], time));
            }
        }
    }
}
