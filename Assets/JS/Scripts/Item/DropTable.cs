using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropItem
{
    string ItemName { get; }
    Sprite Image { get; }
}

[System.Serializable]
public class DropEntry
{
    //public ItemData_JS item; // 아이템 SO
    public ScriptableObject item; // 임시 사용
    public float weight; // 가중치(확률) 설정

    public IDropItem DropItem => item as IDropItem;
}

[CreateAssetMenu(fileName = "DropTable", menuName = "Item/DropTable")]
public class DropTable : ScriptableObject
{
    public List<DropEntry> entries;
}