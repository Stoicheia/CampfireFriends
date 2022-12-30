using Core;
using UnityEngine;

namespace Minigame
{
    public class MinigameStarterTest : MonoBehaviour
    {
        [SerializeField] private MinigameDefinition _config;

        public void StartMinigame()
        {
            SceneCameraNavigator.Instance.TransitionToMinigame(_config);
        }
    }
}