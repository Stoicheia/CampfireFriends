using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Minigame
{
    public class ScanLineGraphics : MonoBehaviour
    {
        [SerializeField] private ScanLine _scanLine;
        [SerializeField] private Transform _top;
        [SerializeField] private Transform _bottom;
        [SerializeField] private Transform _hitUISpawnPlace;
        [SerializeField] private float _spawnBeforeSeconds;
        [SerializeField] private float _despawnAfterSeconds;
        [SerializeField] private SpriteRenderer _spritePrefab;

        [Space] 
        [SerializeField] private SpriteRenderer _hitSprite;
        [SerializeField] private Sprite _inactiveSprite;
        [SerializeField] private Sprite _activeSprite;

        private Dictionary<ScanEvent, SpriteRenderer> _eventSprites;
        private float Time => _scanLine.Time;
        public ScanLine Line => _scanLine;
        public Transform HitUISpawnPlace => _hitUISpawnPlace;

        private void Awake()
        {
            _eventSprites = new Dictionary<ScanEvent, SpriteRenderer>();
        }

        private void OnEnable()
        {
            _scanLine.OnInit += Init;
            _scanLine.OnHit += Hit;
        }
        
        private void OnDisable()
        {
            _scanLine.OnInit -= Init;
            _scanLine.OnHit -= Hit;
            
            foreach(var e in _eventSprites) Destroy(e.Value.gameObject);
            _eventSprites.Clear();
        }

        private void Update()
        {
            _hitSprite.sprite = _scanLine.IsPressed ? _activeSprite : _inactiveSprite;
            
            foreach (var s in _eventSprites)
            {
                ScanEvent @event = s.Key;
                SpriteRenderer sprite = s.Value;

                float timeSpawn = @event.TimeSeconds - _spawnBeforeSeconds;
                float timeDespawn = @event.TimeSeconds + _despawnAfterSeconds;

                sprite.enabled = Time >= timeSpawn && Time <= timeDespawn;
                if (sprite.enabled)
                {
                    float t = Utility.InverseLerpUnclamped(timeSpawn, @event.TimeSeconds, Time);
                    sprite.transform.position = Vector3.LerpUnclamped(_top.position, _bottom.position, t);
                    sprite.sprite = @event.RequestedObject.Sprite;
                }
            }
        }

        private void Init()
        {
            foreach (var es in _eventSprites)
            {
                Destroy(es.Value.gameObject);
            }
            _eventSprites.Clear();
            foreach (var s in _scanLine.Events)
            {
                SpriteRenderer obj = Instantiate(_spritePrefab, transform).GetComponent<SpriteRenderer>();
                _eventSprites.Add(s, obj);
                obj.enabled = false;
            }
            _spawnBeforeSeconds = _scanLine.ApproachRate;
        }

        private void Hit(ScanEvent scanEvent, float f)
        {
            if (scanEvent == null) return;
            _eventSprites[scanEvent].gameObject.SetActive(false);
        }
    }
}