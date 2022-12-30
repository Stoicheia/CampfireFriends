using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog[]", menuName = "ScriptableObjects/Dialogs")]
public class DialogData : ScriptableObject
{
    [Range(1, 10)]public int randomQuanityMax = 3;
    [Space]
    [Range(1, 30)]public int requestAmount = 3;

    [Header("Randomness")]
    public ItemData[] possibleGivenItems;
    [Space]
    public ItemData[] ChosenItems;
    public int[] ChosenQuanityForEach;
    
    [System.Serializable]
    public class EndResults
    {
        public int resultPercent = 50;
        [Space]
        public string EndDialog;
    }

    [Header("End Dialoge")]
    public List<EndResults> endResultDialoge = new List<EndResults>();

    public void Generate()
    {
        Random.InitState(Random.Range(1111111, 9999999));

        ChosenItems = new ItemData[requestAmount];
        for (int i = 0; i < requestAmount; i++)
        {
            ChosenItems[i] = possibleGivenItems[Random.Range(0, possibleGivenItems.Length)];
        }

        ChosenQuanityForEach = new int[requestAmount];
        for (int i = 0; i < requestAmount; i++)
        {
            ChosenQuanityForEach[i] = Random.Range(1, randomQuanityMax);
        }
    }

    public void Reset()
    {
        ChosenQuanityForEach = null;
        ChosenItems = null;
    }
}
