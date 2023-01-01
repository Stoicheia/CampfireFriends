using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Entity;
using UnityEngine;
using UnityEngine.UI;

public enum AnimalType
{
    Bear, Squirrel, Fox
}

[RequireComponent(typeof(AudioSource))]
public class DialogeMan : MonoBehaviour
{
    public int currentIndex = 0;
    public ResultsData results;
    bool isTalking;
    bool finishedTalking = false;

    [Space]

    public TMPro.TextMeshProUGUI dialogText;
    public DialogData dialogData;

    string text_preview;

    [Header("Extra")]
    public KeyCode SkipButton;
    public float letterDelay;

    public MinigameGiver MinigameGiver;
    public AnimalType AnimalType;

    public DialogueSequenceAsset FixedInitialDialogue;
    public AccuracyDialogueAsset FinalDialogue;

    private List<string> _dialogueLinesInitial;
    private List<string> _dialogueLinesFinal;

    public MainGameManager MyManager;

    public Image StandingSprite;
    public Image SittingSprite;
    public RectTransform TextBubble;

    [Space] public AudioClip _noises;
    private AudioSource _audio;
    private void Awake()
    {
        _dialogueLinesInitial = FixedInitialDialogue.Lines;
        _dialogueLinesFinal = new List<string>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _audio.clip = _noises;
        _audio.Play();
        _audio.Pause();
    }

    private void Update()
    {
        dialogText.text = text_preview;
    }

    public void RunInitial()
    {
        StartCoroutine(RunLines(_dialogueLinesInitial, false));
    }
    
    public void RunFinal()
    {
        float accuracy = PlayerData.Instance.GetAccuracy(AnimalType);
        string line = FinalDialogue.GetLine(accuracy);
        _dialogueLinesFinal.Add(line);

        StartCoroutine(RunLines(_dialogueLinesFinal, true));
    }

    IEnumerator RunLines(List<string> lines, bool end)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            StartCoroutine(RunLine(lines[i]));
            while (true)
            {
                if (!isTalking && Input.anyKeyDown && !Input.GetMouseButtonDown(0))
                {
                    break;
                }

                yield return null;
            }
        }

        if (end)
        {
            MyManager.ShowEndButton();
        }
        else
        {
            MyManager.ShowStartButton(MinigameGiver.Config);
        }
    }

    IEnumerator RunLine(string d)
    {
        text_preview = "";
        isTalking = true;
        _audio.UnPause();
        foreach (char letter in d)
        {
            text_preview += letter;
            yield return new WaitForSeconds(letterDelay);
            isTalking = true;
        }
        _audio.Pause();
        isTalking = false;
        finishedTalking = true;
    }

    private IEnumerator WaitForKeyPress()
    {
        bool done = false;
        while (!done)
        {
            if (Input.GetKeyDown(SkipButton))
            {
                done = true;
            }
            yield return null;
        }
    }

    IEnumerator CheckResults(int index)
    {
        string anwserBasedOnPercent = dialogData.endResultDialoge[results.accuracy[index] <= dialogData.endResultDialoge[0].resultPercent ? 0 : 1].EndDialog;
        string dialog_ = anwserBasedOnPercent;

        foreach (char letter in dialog_)
        {
            text_preview += letter;
            yield return new WaitForSeconds(letterDelay);
        }

        yield return null;
    }
}
