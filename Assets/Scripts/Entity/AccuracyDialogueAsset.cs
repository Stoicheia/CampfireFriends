using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(fileName = "Accuracy Dialogue", menuName = "Accuracy Dialogue", order = 0)]
    public class AccuracyDialogueAsset : ScriptableObject
    {
        [Serializable]
        public class AccuracyDialoguePair
        {
            public float MinAccuracy;
            [TextArea(2, 8)]  public string Line;
        }
        
        public List<AccuracyDialoguePair> Dialogue;

        public string GetLine(float acc)
        {
            var line = Dialogue[0].Line;
            foreach (var l in Dialogue)
            {
                if (acc >= l.MinAccuracy)
                {
                    line = l.Line;
                }
            }

            return line;
        }
    }
}