using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Minigame
{
    public class MinigameManager : MonoBehaviour
    {
        public event Action OnInit;
        public static event Action<ScanLine, bool, HitResult> OnHit;
        public static event Action<ScanLine, bool, ScanEvent> OnMiss;
        public event Action<MinigameResults> OnEnd;
        
        [SerializeField] private MinigameDefinition _minigameConfig;
        [SerializeField] private List<ScanLine> _scanLines;
        [SerializeField] private RhythmEngine _rhythmEngine;

        private List<ItemData> _itemsToSpawn;
        private bool _started;

        private Dictionary<ItemData, float> _itemsCollected;
        private Dictionary<ItemData, int> _goodItemCounts;

        public List<ItemData> GoodItems => _minigameConfig.GoodItems;
        public float Bpm => _minigameConfig.Bpm;
        public AnimalType CurrentAnimal => _minigameConfig.Animal;

        private int TotalItemNumber =>
            _scanLines.Count * _minigameConfig.TotalBeatCount / _minigameConfig.Subdivisions;

        public float SumOfBadScores => _itemsCollected
            .Where(x => !GoodItems.Contains(x.Key))
            .Select(x => x.Value)
            .Aggregate((x, y) => x + y);

        public void SetConfig(MinigameDefinition config)
        {
            _minigameConfig = config;
        }

        private void OnEnable()
        {
            foreach (var line in _scanLines)
            {
                line.OnHit += ProcessHit;
                line.OnMiss += ProcessMiss;
                line.ApproachRate = _minigameConfig.ApproachSeconds;
            }

            _rhythmEngine.OnSongEnd += HandleEnd;

            _itemsCollected = new Dictionary<ItemData, float>();
            PopulateGoodItems();
            ResetAllLines();
            GenerateItemsToSpawn(TotalItemNumber);
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
            /*if (Input.GetKey(KeyCode.Space) && _started)
            {
                HandleEnd();
            }*/
        }

        public void StartGame(float countdown)
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
            _rhythmEngine.StartAudio(countdown);
            _started = true;
            OnInit?.Invoke();
        }

        public float GetScore(ItemData item)
        {
            return _itemsCollected[item];
        }
        
        public float GetMaxScore(ItemData item)
        {
            var goodItems = _goodItemCounts;
            foreach (var gi in goodItems)
            {
                if (gi.Key.Equals(item))
                {
                    return gi.Value * 3;
                }
            }

            return -1;
        }

        public float GetSongProgress()
        {
            return _rhythmEngine.Finished ? 1 : _rhythmEngine.CurrentTimeSeconds / _rhythmEngine.TotalTimeSeconds;
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
            List<ItemData> toSpawn = new List<ItemData>();
            foreach (var pair in _goodItemCounts)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    toSpawn.Add(pair.Key);
                }
            }

            while (toSpawn.Count < totalNumber)
            {
                toSpawn.Add(_minigameConfig.GetRandomBadItem());
            }
            
            Utility.Shuffle(toSpawn);
            _itemsToSpawn = toSpawn;
        }

        private void PopulateGoodItems()
        {
            float density = _minigameConfig.GoodItemDensity +
                            Random.Range(-_minigameConfig.GoodItemVariance, _minigameConfig.GoodItemVariance);
            int goodNumber = (int)(TotalItemNumber * density);
            _goodItemCounts = new Dictionary<ItemData, int>();
            foreach (var gi in _minigameConfig.GoodItems)
            {
                _goodItemCounts.Add(gi, 0);
            }
            for (int i = 0; i < goodNumber; i++)
            {
                ItemData goodItem = GoodItems[Random.Range(0, GoodItems.Count)];
                _goodItemCounts[goodItem]++;
            }
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
            for (int i = 0; i < 4 * Math.Floor((float)_itemsToSpawn.Count/4); i++)
            {
                float time = _minigameConfig.OffsetSeconds + _minigameConfig.Subdivisions * (int)(i/4) * 60 / _minigameConfig.Bpm;
                var line = _scanLines[i % _scanLines.Count];
                line.AddEvent(new ScanEvent(_itemsToSpawn[i], time, line));
            }
        }

        private void HandleEnd()
        {
            Dictionary<ItemData, int> goodItems = new Dictionary<ItemData, int>();
            foreach (var kvp in _itemsCollected)
            {
                if (GoodItems.Contains(kvp.Key))
                {
                    goodItems.Add(kvp.Key, (int)kvp.Value);
                }
            }
            Dictionary<ItemData, int> goodItemTotals = new Dictionary<ItemData, int>();
            int badItemTotal = 0;
            foreach (var item in GoodItems)
            {
                goodItemTotals[item] = 0;
            }
            foreach (var item in _itemsToSpawn)
            {
                if (GoodItems.Contains(item))
                {
                    goodItemTotals[item] += 3;
                }
                else
                {
                    badItemTotal++;
                }
            }

            _results = new MinigameResults(goodItems, goodItemTotals, (int) SumOfBadScores, badItemTotal);
            _started = false;
            if(_rhythmEngine.IsPlaying) _rhythmEngine.StopAudio();
            OnEnd?.Invoke(_results);
        }

        private MinigameResults _results;
        public void SaveAndExit()
        {
            PlayerData.Instance.AddResult(_minigameConfig.Animal, _results);
            SceneCameraNavigator.Instance.TransitionToMain();
        }
    }
}
