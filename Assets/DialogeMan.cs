using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Update()
    {
        if (Input.GetKeyDown(SkipButton) && !finishedTalking && !isTalking)
            StartCoroutine(RunDialog());

        dialogText.text = text_preview;
    }


    IEnumerator RunDialog()
    {
        string ItemsRequired = "";
        for (int i = 0; i < dialogData.requestAmount; i++)
        {
            ItemsRequired = ItemsRequired + dialogData.ChosenQuanityForEach[i] + " " + (dialogData.ChosenQuanityForEach[i] == 1 ? dialogData.ChosenItems[i].itemSingularName : dialogData.ChosenItems[i].itemPluralName) + ", ";
        }

        string dialog_ = $"Hello There i Will Need\n {ItemsRequired} \nThanks Have Fun!";

        foreach (char letter in dialog_)
        {
            text_preview += letter;
            yield return new WaitForSeconds(letterDelay);
            isTalking = true;
        }
        isTalking = false;

        finishedTalking = true;
        yield return new WaitForSeconds(2f);
        text_preview = "";
        StartCoroutine(CheckResults(currentIndex));
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
