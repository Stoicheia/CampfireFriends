using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(fileName = "Dialogue Sequence Asset", menuName = "Dialogue Sequence", order = 0)]
    public class DialogueSequenceAsset : ScriptableObject
    {
        [TextArea(2, 8)] public List<string> Lines;
    }
}