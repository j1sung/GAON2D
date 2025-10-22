using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { CoreMain, CoreSub, Blueprint }

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData_JS : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite image;
}