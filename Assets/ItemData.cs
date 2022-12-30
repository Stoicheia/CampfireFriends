using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item[]", menuName = "ScriptableObjects/Items")]
public class ItemData : ScriptableObject
{
    public string Name => itemSingularName;
    public Sprite Sprite => itemIcon;
    public Sprite itemIcon;
    [Space]
    public string itemSingularName;
    public string itemPluralName;
}
