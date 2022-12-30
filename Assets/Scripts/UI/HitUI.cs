using System;
using System.Collections.Generic;
using Minigame;
using UnityEngine;

namespace UI
{
    public class HitUI : MonoBehaviour
    {
        [SerializeField] private HitUIPopup _hitPrefab;
        [SerializeField] private List<ScanLineGraphics> _lineGraphics;
        [Space]
        [SerializeField] private string _perfectText;
        [SerializeField] private Color _perfectColor;
        [SerializeField] private string _hitText;
        [SerializeField] private Color _hitColor;
        [SerializeField] private string _missText;
        [SerializeField] private Color _missColor;
        [SerializeField] private string _junkText;
        [SerializeField] private Color _junkColor;

        private Dictionary<ScanLine, ScanLineGraphics> _lineToGraphic;

        private void Start()
        {
            _lineToGraphic = new Dictionary<ScanLine, ScanLineGraphics>();
            foreach (var l in _lineGraphics)
            {
                _lineToGraphic.Add(l.Line, l);
            }
        }

        private void OnEnable()
        {
            MinigameManager.OnHit += HandleHit;
            MinigameManager.OnMiss += HandleMiss;
        }
        
        private void OnDisable()
        {
            MinigameManager.OnHit -= HandleHit;
            MinigameManager.OnMiss -= HandleMiss;
        }

        private void HandleHit(ScanLine scanLine, bool good, HitResult accuracy)
        {
            HitUIPopup obj = Instantiate(_hitPrefab, _lineToGraphic[scanLine].HitUISpawnPlace);
            (string, Color) res = (good, accuracy) switch
            {
                (true, HitResult.Perfect) => (_perfectText, _perfectColor),
                (true, HitResult.Hit) => (_hitText, _hitColor),
                (false, _) => (_junkText, _junkColor),
                _ => (_junkText, _junkColor)
            };
            obj.SetText(res.Item1);
            obj.SetColor(res.Item2);
        }

        private void HandleMiss(ScanLine scanLine, bool good, ScanEvent scanEvent)
        {
            if (!good) return;
            HitUIPopup obj = Instantiate(_hitPrefab, _lineToGraphic[scanLine].HitUISpawnPlace);
            obj.SetText(_missText);
            obj.SetColor(_missColor);
        }
        
    }
}