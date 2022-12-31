using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private List<string> _sceneNames;
        private void Start()
        {
            foreach(var n in _sceneNames)
            {
                SceneManager.LoadScene(n, LoadSceneMode.Additive);
            }
        }
    }
}