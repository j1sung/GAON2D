/// 각 슬롯을 관리하는 데이터 클래스.
/// 설계도(blueprint)와 코어(Cores)를 저장하며,
/// 슬롯 용량/상태를 판별하는 유틸 메서드를 제공한다.

using System.Collections.Generic;

[System.Serializable]
public class Slot
{
    public const int Capacity = 2;

    public BlueprintSO blueprint;                         // 설계도
    public List<CoreSO> Cores = new List<CoreSO>(Capacity); // 코어 리스트(초기 capacity만 지정)
    public WeaponSO combinedWeapon;                       // 조합 결과

    public bool IsEmpty()         { return combinedWeapon == null && blueprint == null && Cores.Count == 0; }
    public bool HasBlueprint()    { return blueprint != null; }
    public bool CanAddBlueprint() { return combinedWeapon == null && blueprint == null; }
    public bool CanAddCore()      { return combinedWeapon == null && Cores.Count < Capacity; }
    public bool ReadyToCombine()  { return combinedWeapon == null && blueprint != null && Cores.Count == Capacity; }

    // 슬롯 설계도, 코어 비우기 (조합 결과는 유지/초기화 정책에 따라 외부에서 결정)
    public void ClearSlot()
    {
        blueprint = null;
        Cores.Clear();
    }
}