using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame
{
    public class ScanLineGraphics : MonoBehaviour
    {
        [SerializeField] private ScanLine _scanLine;
        [SerializeField] private Transform _top;
        [SerializeField] private Transform _bottom;
        [SerializeField] private float _spawnBeforeSeconds;
        [SerializeField] private float _despawnAfterSeconds;
        [SerializeField] private SpriteRenderer _spritePrefab;

        private Dictionary<ScanEvent, SpriteRenderer> _eventSprites;
        private float Time => _scanLine.Time;

        private void Awake()
        {
            _eventSprites = new Dictionary<ScanEvent, SpriteRenderer>();
        }

        private void OnEnable()
        {
            _scanLine.OnInit += Init;
        }
        
        private void OnDisable()
        {
            _scanLine.OnInit -= Init;
        }

        private void Update()
        {
            foreach (var s in _eventSprites)
            {
                ScanEvent @event = s.Key;
                SpriteRenderer sprite = s.Value;

                float timeSpawn = s.Key.TimeSeconds - _spawnBeforeSeconds;
                float timeDespawn = s.Key.TimeSeconds + _despawnAfterSeconds;

                sprite.enabled = Time >= timeSpawn && Time <= timeDespawn;
                if (sprite.enabled)
                {
                    float t = Mathf.InverseLerp(timeSpawn, s.Key.TimeSeconds, Time);
                    sprite.transform.position = Vector3.LerpUnclamped(_top.position, _bottom.position, t);
                }
            }
        }

        private void Init()
        {
            foreach (var s in _scanLine.Events)
            {
                SpriteRenderer obj = Instantiate(_spritePrefab, transform).GetComponent<SpriteRenderer>();
                _eventSprites.Add(s, obj);
                obj.enabled = false;
            }
        }
    }
}