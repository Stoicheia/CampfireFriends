﻿using System;
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

        public NavigationElements(Camera mainSceneCamera, Canvas mainSceneCanvas, Camera minigameCamera, Canvas minigameCanvas, Animator transitionWindowIntoMinigame, Animator transitionWindowOutOfMinigame, MinigameManager minigameManager, MinigameAnimator minigameAnimator)
        {
            MainSceneCamera = mainSceneCamera;
            MainSceneCanvas = mainSceneCanvas;
            MinigameCamera = minigameCamera;
            MinigameCanvas = minigameCanvas;
            TransitionWindowIntoMinigame = transitionWindowIntoMinigame;
            TransitionWindowOutOfMinigame = transitionWindowOutOfMinigame;
            MinigameManager = minigameManager;
            MinigameAnimator = minigameAnimator;
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
                MinigameAnimator == null ? add.MinigameAnimator : MinigameAnimator
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
            NavigationData.MainSceneCamera.gameObject.SetActive(false);
            NavigationData.MainSceneCanvas.gameObject.SetActive(false);
            NavigationData.MinigameCamera.gameObject.SetActive(true);
            NavigationData.MinigameCanvas.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            NavigationData.TransitionWindowIntoMinigame.Play("out");
            yield return new WaitForSeconds(1);
            NavigationData.MinigameAnimator.Begin();
            yield return new WaitForSeconds(1);
            NavigationData.TransitionWindowIntoMinigame.gameObject.SetActive(false);
        }
        
        private IEnumerator TransitionToMainSequence()
        {
            NavigationData.TransitionWindowOutOfMinigame.gameObject.SetActive(true);
            NavigationData.TransitionWindowOutOfMinigame.Play("in");
            yield return new WaitForSeconds(1);
            NavigationData.MainSceneCamera.gameObject.SetActive(false);
            NavigationData.MainSceneCanvas.gameObject.SetActive(false);
            NavigationData.MinigameCamera.gameObject.SetActive(true);
            NavigationData.MinigameCanvas.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            NavigationData.TransitionWindowOutOfMinigame.Play("out");
            yield return new WaitForSeconds(2);
            NavigationData.TransitionWindowOutOfMinigame.gameObject.SetActive(false);
        }
    }
}