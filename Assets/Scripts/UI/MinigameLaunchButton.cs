using Core;
using Minigame;
using UnityEngine;

namespace UI
{
    public class MinigameLaunchButton : MonoBehaviour
    {
        public MinigameDefinition ToLaunch;
        public void Launch()
        {
            SceneCameraNavigator.Instance.TransitionToMinigame(ToLaunch);
        }       
    }
}