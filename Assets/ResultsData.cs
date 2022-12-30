using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Results", menuName = "ScriptableObjects/Results")]
public class ResultsData : ScriptableObject
{
    public int[] accuracy = new int[3];
}
