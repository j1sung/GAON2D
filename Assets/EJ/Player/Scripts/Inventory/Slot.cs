/// 각 슬롯을 관리하는 클래스
/// 설계도=와 코어=를 저장
/// 슬롯 용량/상태를 판별하는 메서드를 제공한다.

using System.Collections.Generic;

[System.Serializable]
public class Slot
{
    public const int Capacity = 2;

    public BlueprintSO blueprint;                         // 설계도
    public List<CoreSO> Cores = new List<CoreSO>(Capacity); // 코어 리스트
    public WeaponSO combinedWeapon;                       // 조합 무기

    public bool IsEmpty()         { return combinedWeapon == null && blueprint == null && Cores.Count == 0; }
    public bool HasBlueprint()    { return blueprint != null; }
    public bool CanAddBlueprint() { return combinedWeapon == null && blueprint == null; }
    public bool CanAddCore()      { return combinedWeapon == null && Cores.Count < Capacity; }
    public bool ReadyToCombine()  { return combinedWeapon == null && blueprint != null && Cores.Count == Capacity; }

    // 슬롯 설계도, 코어 비우기
    public void ClearSlot()
    {
        blueprint = null;
        Cores.Clear();
    }
}