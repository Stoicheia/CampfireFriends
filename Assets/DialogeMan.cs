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

    private List<string> _dialogueLinesInitial;
    private List<string> _dialogueLinesFinal;

    public MainGameManager MyManager;

    public Image StandingSprite;
    public Image SittingSprite;
    public RectTransform TextBubble;
    private void Awake()
    {
        _dialogueLinesInitial = FixedInitialDialogue.Lines;
        _dialogueLinesFinal = new List<string>();
        switch (AnimalType)
        {
            case AnimalType.Bear:
                _dialogueLinesFinal.Add("sdsds");
                break;
        }
    }

    private void Update()
    {
        dialogText.text = text_preview;
    }

    public void RunInitial()
    {
        StartCoroutine(RunLines(_dialogueLinesInitial));
    }
    
    public void RunFinal()
    {
        StartCoroutine(RunLines(_dialogueLinesFinal));
    }

    IEnumerator RunLines(List<string> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            StartCoroutine(RunLine(lines[i]));
            while (true)
            {
                if (!isTalking && Input.anyKeyDown)
                {
                    break;
                }

                yield return null;
            }
        }

        MyManager.ShowStartButton(MinigameGiver.Config);
    }

    IEnumerator RunLine(string d)
    {
        text_preview = "";
        isTalking = true;
        foreach (char letter in d)
        {
            text_preview += letter;
            yield return new WaitForSeconds(letterDelay);
            isTalking = true;
        }
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
