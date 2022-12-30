using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigame
{
    
    [Serializable]
    public class ScanEvent
    {
        public PrimitiveItem RequestedObject { get; set; }
        public float TimeSeconds { get; set; }

        public ScanEvent(PrimitiveItem item, float time)
        {
            RequestedObject = item;
            TimeSeconds = time;
        }
    }   
    public class ScanLine : MonoBehaviour
    {
        public event Action OnInit;
        public event Action<ScanEvent, float> OnHit; 

        private RhythmEngine _engine;
        private List<ScanEvent> _events;
        private int _ptr;

        private ScanEvent _lastEvent;
        private ScanEvent _nextEvent;

        public float Time => _engine.CurrentTimeSeconds;
        public List<ScanEvent> Events => _events;

        private void Update()
        {
            while(_ptr < _events.Count && Time > _events[_ptr + 1].TimeSeconds)
            {
                _ptr++;
            }

            _lastEvent = _events[_ptr];
            _nextEvent = _events[_ptr + 1];
        }
        
        public void Reset()
        {
            _ptr = 0;
            _events = new List<ScanEvent>();
        }

        public void AddEvent(ScanEvent e)
        {
            _events.Add(e);
        }

        public void Init()
        {
            SortEventsByTime();
            OnInit?.Invoke();
        }
        
        // return value: (event, time of hit)
        public (ScanEvent, float) Hit()
        {
            ScanEvent closestEvent =
                Time - _lastEvent.TimeSeconds < _nextEvent.TimeSeconds - Time ? _lastEvent : _nextEvent;
            OnHit?.Invoke(closestEvent, Time);
            return (closestEvent, Time);
        }
        
        private void SortEventsByTime()
        {
            _events.Sort((x,y) => x.TimeSeconds.CompareTo(y.TimeSeconds));
        }
    }
}