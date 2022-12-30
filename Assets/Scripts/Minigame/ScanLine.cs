using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigame
{
    
    [Serializable]
    public class ScanEvent
    {
        public ScanLine FromLine { get; set; }
        public PrimitiveItem RequestedObject { get; set; }
        public float TimeSeconds { get; set; }

        public ScanEvent(PrimitiveItem item, float time, ScanLine line)
        {
            RequestedObject = item;
            TimeSeconds = time;
            FromLine = line;
        }
    }   
    public class ScanLine : MonoBehaviour
    {
        public event Action OnInit;
        public event Action<ScanEvent, float> OnHit;
        public event Action<ScanEvent, float> OnMiss;

        [SerializeField] private RhythmEngine _engine;
        [SerializeField] private KeyCode _key;
        private List<ScanEvent> _events;
        private int _ptr;
        private int _missPtr;

        private ScanEvent _lastEvent;
        private ScanEvent _nextEvent;

        private bool _started;
        private ScanEvent _lastTriggeredEvent;

        public float Time => _engine.CurrentTimeSeconds;
        public List<ScanEvent> Events => _events;
        public bool IsPressed => Input.GetKey(_key);
        public float LeniencySeconds { get; set; }

        private void Awake()
        {
            _started = false;
        }

        private void Update()
        {
            if (!_started) return;
            while(_ptr < _events.Count && Time > _events[_ptr + 1].TimeSeconds)
            {
                _ptr++;
            }

            _lastEvent = _events[_ptr];
            _nextEvent = _events[_ptr + 1];

            if (Input.GetKeyDown(_key))
            {
                Hit();
            }

            while (Time > _events[_missPtr].TimeSeconds + LeniencySeconds)
            {
                OnMiss?.Invoke(_events[_missPtr], Time);
                _missPtr++;
            }
        }
        
        public void Reset()
        {
            _ptr = 0;
            _missPtr = 0;
            _events = new List<ScanEvent>();
        }

        public void AddEvent(ScanEvent e)
        {
            _events.Add(e);
        }

        public void Init()
        {
            SortEventsByTime();
            _started = true;
            OnInit?.Invoke();
        }
        
        // return value: (event, time of hit)
        public (ScanEvent, float) Hit()
        {
            ScanEvent closestEvent =
                Time - _lastEvent.TimeSeconds < _nextEvent.TimeSeconds - Time ? _lastEvent : _nextEvent;
            
            if (_lastTriggeredEvent != null)
            {
                if (_lastTriggeredEvent.Equals(_lastEvent)) closestEvent = _nextEvent;
                if (_lastTriggeredEvent.Equals(_nextEvent)) closestEvent = null;
            }

            if (closestEvent != null && Mathf.Abs(Time - closestEvent.TimeSeconds) <= LeniencySeconds)
            {
                _lastTriggeredEvent = closestEvent;
                _missPtr++;
                OnHit?.Invoke(closestEvent, Time);
            }

            return (closestEvent, Time);
        }
        
        private void SortEventsByTime()
        {
            _events.Sort((x,y) => x.TimeSeconds.CompareTo(y.TimeSeconds));
        }
    }
}