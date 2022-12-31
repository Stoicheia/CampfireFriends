using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogData))]

public class DialogeDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogData dialoge = (DialogData)target;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate"))
        {
            dialoge.Generate();
        }

        if (GUILayout.Button("Reset"))
        {
            dialoge.Reset();
        }

        GUILayout.EndHorizontal();
    }
}
