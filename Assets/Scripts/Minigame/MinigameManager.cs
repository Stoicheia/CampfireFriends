using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Minigame
{
    public class MinigameManager : MonoBehaviour
    {
        [SerializeField] private MinigameDefinition _minigameConfig;
        [SerializeField] private List<ScanLine> _scanLines;
        [SerializeField] private RhythmEngine _rhythmEngine;

        private List<PrimitiveItem> _itemsToSpawn;
        private bool _started;

        private void OnEnable()
        {
            foreach (var line in _scanLines)
            {
                line.OnHit += ProcessHit;
            }

            GenerateItemsToSpawn(_scanLines.Count * _minigameConfig.TotalBeatCount / 4);
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
            _rhythmEngine.SetParams(_minigameConfig);
            _rhythmEngine.StartAudio();
        }

        private void ProcessHit(ScanEvent @event, float t)
        {
            Debug.Log($"{@event.RequestedObject.Name} collected at time {t}");
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

        private void DistributeToLines()
        {
            for (int i = 0; i < _itemsToSpawn.Count; i++)
            {
                float time = _minigameConfig.OffsetSeconds + 4 * _minigameConfig.Bpm * (i/4) / 60;
                _scanLines[i % _scanLines.Count].AddEvent(new ScanEvent(_itemsToSpawn[i], time));
            }
        }
    }
}
