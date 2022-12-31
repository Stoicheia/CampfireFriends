using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ProfilesButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private List<Image> _spriteSlots;
        [SerializeField] private List<Sprite> _inactives;
        [SerializeField] private List<Sprite> _actives;

        [SerializeField] private RectTransform _profile;

        private void Start()
        {
            SetSpritesInactive();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetSpritesActive();
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            SetSpritesInactive();
        }
        private void SetSpritesActive()
        {
            for (int i = 0; i < _spriteSlots.Count; i++)
            {
                _spriteSlots[i].sprite = _actives[i];
            }
        }

        private void SetSpritesInactive()
        {
            for (int i = 0; i < _spriteSlots.Count; i++)
            {
                _spriteSlots[i].sprite = _inactives[i];
            }
        }

        public void ToggleProfile()
        {
            _profile.gameObject.SetActive(!_profile.gameObject.activeSelf);
        }
    }
}