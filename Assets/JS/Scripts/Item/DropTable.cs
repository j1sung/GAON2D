using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropEntry
{
    //public ItemData_JS item; // 아이템 SO
    public GameObject prefab; // 직접 프리펩 넣기
    public float weight; // 가중치(확률) 설정
}

[CreateAssetMenu(fileName = "DropTable", menuName = "Item/DropTable")]
public class DropTable : ScriptableObject
{
    public List<DropEntry> entries;
}