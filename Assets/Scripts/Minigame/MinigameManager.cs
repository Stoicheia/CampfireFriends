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
        public static event Action<ScanLine, bool, HitResult> OnHit;
        public static event Action<ScanLine, bool, ScanEvent> OnMiss;
        
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
                line.OnMiss += ProcessMiss;
            }

            _rhythmEngine.OnSongEnd += HandleEnd;

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
                line.OnMiss -= ProcessMiss;
            }
            
            _rhythmEngine.OnSongEnd -= HandleEnd;
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
                t.LeniencySeconds = _minigameConfig.LeniencySeconds;
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
            if (@event == null) return;
            float error = t - @event.TimeSeconds;
            Debug.Log($"{@event.RequestedObject.Name} collected with error {error}");

            HitResult result;
            if (Mathf.Abs(error) <= _minigameConfig.LeniencyPerfectSeconds) result = HitResult.Perfect;
            else if (Mathf.Abs(error) <= _minigameConfig.LeniencySeconds) result = HitResult.Hit;
            else result = HitResult.Miss;

            bool isGood = GoodItems.Contains(@event.RequestedObject);

            float score = (isGood, result) switch
            {
                (true, HitResult.Perfect) => 3,
                (true, HitResult.Hit) => 1,
                (false, _) => 1,
                _ => 0
            };

            _itemsCollected[@event.RequestedObject] += score;
            Debug.Log($"{@event.RequestedObject.Name} score = {_itemsCollected[@event.RequestedObject]}");
            
            OnHit?.Invoke(@event.FromLine, isGood, result);
        }

        private void ProcessMiss(ScanEvent scanEvent, float f)
        {
            bool isGood = GoodItems.Contains(@scanEvent.RequestedObject);
            OnMiss?.Invoke(scanEvent.FromLine, isGood, scanEvent);
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
                toSpawn.Add(_minigameConfig.GetRandomBadItem());
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
                var line = _scanLines[i % _scanLines.Count];
                line.AddEvent(new ScanEvent(_itemsToSpawn[i], time, line));
            }
        }

        private void HandleEnd()
        {
            Debug.Log("End.");
        }
    }
}
