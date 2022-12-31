using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    ItemData item;

    private void OnEnable()
    {
        item = target as ItemData;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (item.itemIcon == null)
            return;

        Texture2D texture = AssetPreview.GetAssetPreview(item.itemIcon);
        GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(80));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}
