using System;
using Minigame;
using UnityEngine;

namespace Core
{
    public class NavigatorWriter : MonoBehaviour
    {
        public NavigationElements NavigationData;

        private void Start()
        {
            SceneCameraNavigator.Instance.AddData(NavigationData);
        }
    }
}