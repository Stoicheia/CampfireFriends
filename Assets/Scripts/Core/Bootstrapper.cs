using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private List<string> _sceneNames;
        [SerializeField] private RectTransform _disableOnLoad;
        private void Start()
        {
            foreach(var n in _sceneNames)
            {
                var t = SceneManager.LoadSceneAsync(n, LoadSceneMode.Additive);
                t.completed += _ => _disableOnLoad.gameObject.SetActive(false);
            }
            
            
        }
    }
}