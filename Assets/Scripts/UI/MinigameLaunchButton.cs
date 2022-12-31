using System.Collections.Generic;
using Core;
using Minigame;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MinigameLaunchButton : MonoBehaviour
    {
        public MinigameDefinition ToLaunch;
        public AudioSource Audio;
        public List<string> UnloadScenes;
        public void Launch()
        {
            SceneCameraNavigator.Instance.TransitionToMinigame(ToLaunch);
        }

        public void End()
        {
            foreach (var s in UnloadScenes)
            {
                SceneManager.UnloadSceneAsync(s);
            }

            SceneManager.LoadScene("EndScreen", LoadSceneMode.Additive);
        }

        public void PlaySound(AudioClip c)
        {
            Audio.PlayOneShot(c);
        }
    }
}