/// 각 슬롯을 관리하는 코드이다.
/// 슬롯에 담길 설계도와 코어를 저장한다. 
/// 또한, 슬롯의 용량을 고려하여 획득한 아이템을 어떻게 처리할 것인지를 판별한다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public BlueprintSO blueprint;            // 설계도(
    public List<CoreSO> Cores = new List<CoreSO>(2); // 코어
    public WeaponSO combinedWeapon;                 // 조합 결과

    public bool IsEmpty()         { return combinedWeapon == null && blueprint == null && Cores.Count == 0; }
    public bool HasBlueprint()    { return blueprint != null; }
    public bool CanAddBlueprint() { return combinedWeapon == null && blueprint == null; }
    public bool CanAddCore()  { return combinedWeapon == null && blueprint != null && Cores.Count < 2; }
    public bool ReadyToCombine()    { return combinedWeapon == null && blueprint != null && Cores.Count == 2; }

    // 슬롯 설계도, 코어 비우기
    public void ClearSlot()
    {
        blueprint = null;
        Cores.Clear();
    }
}
