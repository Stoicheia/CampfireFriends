using Core;
using Minigame;
using UnityEngine;

namespace UI
{
    public class MinigameLaunchButton : MonoBehaviour
    {
        public MinigameDefinition ToLaunch;
        public AudioSource Audio;
        public void Launch()
        {
            SceneCameraNavigator.Instance.TransitionToMinigame(ToLaunch);
        }

        public void PlaySound(AudioClip c)
        {
            Audio.PlayOneShot(c);
        }
    }
}