using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/MC")]
public class MC : ScriptableObject, IDropItem
{
    public string itemName;
    public Sprite sprite;

    public string ItemName => itemName;
    public Sprite Image => sprite;
}
