using System;
using System.Collections;
using Minigame;
using UnityEngine;

namespace Core
{
    [Serializable]
    public struct NavigationElements
    {
        public Camera MainSceneCamera;
        public Canvas MainSceneCanvas;
        public Camera MinigameCamera;
        public Canvas MinigameCanvas;
        public Animator TransitionWindowIntoMinigame;
        public Animator TransitionWindowOutOfMinigame;

        public MinigameManager MinigameManager;
        public MinigameAnimator MinigameAnimator;
        public MainGameManager MainManager;

        public NavigationElements(Camera mainSceneCamera, Canvas mainSceneCanvas, Camera minigameCamera, Canvas minigameCanvas, Animator transitionWindowIntoMinigame, Animator transitionWindowOutOfMinigame, MinigameManager minigameManager, MinigameAnimator minigameAnimator, MainGameManager mgm)
        {
            MainSceneCamera = mainSceneCamera;
            MainSceneCanvas = mainSceneCanvas;
            MinigameCamera = minigameCamera;
            MinigameCanvas = minigameCanvas;
            TransitionWindowIntoMinigame = transitionWindowIntoMinigame;
            TransitionWindowOutOfMinigame = transitionWindowOutOfMinigame;
            MinigameManager = minigameManager;
            MinigameAnimator = minigameAnimator;
            MainManager = mgm;
        }

        public NavigationElements Add(NavigationElements add)
        {
            return new NavigationElements(
                MainSceneCamera == null ? add.MainSceneCamera : MainSceneCamera,
                MainSceneCanvas == null ? add.MainSceneCanvas : MainSceneCanvas,
                MinigameCamera == null ? add.MinigameCamera : MinigameCamera,
                MinigameCanvas == null ? add.MinigameCanvas : MinigameCanvas,
                TransitionWindowIntoMinigame == null ? add.TransitionWindowIntoMinigame : TransitionWindowIntoMinigame,
                TransitionWindowOutOfMinigame == null ? add.TransitionWindowOutOfMinigame : TransitionWindowOutOfMinigame,
                MinigameManager == null ? add.MinigameManager : MinigameManager,
                MinigameAnimator == null ? add.MinigameAnimator : MinigameAnimator,
                MainManager == null ? add.MainManager : MainManager
            );
        }
    }
    public class SceneCameraNavigator : MonoBehaviour
    {
        public NavigationElements NavigationData = new NavigationElements();
        public static SceneCameraNavigator Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            NavigationData = new NavigationElements();
            Instance = this;
        }

        public void AddData(NavigationElements e)
        {
            NavigationData = NavigationData.Add(e);
        }

        public void TransitionToMinigame(MinigameDefinition config)
        {
            StartCoroutine(TransitionToMinigameSequence(config));
        }

        public void TransitionToMain()
        {
            StartCoroutine(TransitionToMainSequence());
        }

        private IEnumerator TransitionToMinigameSequence(MinigameDefinition config)
        {
            NavigationData.TransitionWindowIntoMinigame.gameObject.SetActive(true);
            NavigationData.TransitionWindowIntoMinigame.Play("in");
            yield return new WaitForSeconds(1);
            NavigationData.MinigameManager.SetConfig(config);
            NavigationData.MinigameManager.gameObject.SetActive(true);
            NavigationData.MinigameAnimator.gameObject.SetActive(true);
            NavigationData.MainManager.gameObject.SetActive(false);
            NavigationData.MainSceneCamera.gameObject.SetActive(false);
            NavigationData.MainSceneCanvas.gameObject.SetActive(false);
            NavigationData.MinigameCamera.gameObject.SetActive(true);
            NavigationData.MinigameCanvas.gameObject.SetActive(true);
            NavigationData.MinigameAnimator.Begin();
            yield return new WaitForSeconds(0.5f);
            NavigationData.TransitionWindowIntoMinigame.Play("out");
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(1);
            NavigationData.TransitionWindowIntoMinigame.gameObject.SetActive(false);
        }
        
        private IEnumerator TransitionToMainSequence()
        {
            NavigationData.TransitionWindowOutOfMinigame.gameObject.SetActive(true);
            NavigationData.TransitionWindowOutOfMinigame.Play("in");
            yield return new WaitForSeconds(1);
            NavigationData.MainSceneCamera.gameObject.SetActive(true);
            NavigationData.MainSceneCanvas.gameObject.SetActive(true);
            NavigationData.MinigameCamera.gameObject.SetActive(false);
            NavigationData.MinigameCanvas.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            NavigationData.TransitionWindowOutOfMinigame.Play("out");
            NavigationData.MinigameManager.gameObject.SetActive(false);
            NavigationData.MinigameAnimator.gameObject.SetActive(false);
            NavigationData.MainManager.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            NavigationData.TransitionWindowOutOfMinigame.gameObject.SetActive(false);
        }
    }
}