using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HitUIPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _text;
        private void Awake()
        {
            StartCoroutine(DestroyAfterFiveSeconds());
        }

        IEnumerator DestroyAfterFiveSeconds()
        {
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }

        public void SetText(string s)
        {
            _text.text = s;
        }

        public void SetColor(Color c)
        {
            _text.color = c;
        }
    }
}