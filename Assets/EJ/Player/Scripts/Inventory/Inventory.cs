using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int slotCount = 4;
    public Slot[] slots;

    public CombineWeapon weapon;

    void Awake()
    {
        slots = new Slot[slotCount];
        for (int i = 0; i < slotCount; i++)
            slots[i] = new Slot();
    }

    // === 설계도 줍기 ===
    public bool PickupBlueprint(BlueprintSO bp)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Slot s = slots[i];
            if (s.CanAddBlueprint())
            {
                s.blueprint = bp;
                Debug.Log($"슬롯 {i + 1}에 {bp.blueprintName} 추가함.");
                return true;
            }
        }
        Debug.Log("설계도 넣을 자리 없음");
        return false;
    }

    // === 코어 줍기 ===
    public bool PickupCore(CoreSO core)
    {
        if (core == null) return false;

        for (int i = 0; i < slots.Length; i++)
        {
            Slot s = slots[i];
            if (s == null) continue;
            if (!s.CanAddCore()) continue;

            var cores = s.Cores ??= new List<CoreSO>(Slot.Capacity);
            int count = cores.Count;

            // 슬롯 내에 코어가 0개면 바로 추가
            if (count == 0)
            {
                cores.Add(core);
                Debug.Log($"슬롯 {i + 1}에 {core.coreName} 추가함.");
                TryCombine(s, i);
                return true;
            }

            // 코어가 1개 들어있으면 비교
            if (count == 1)
            {
                CoreSO existCore = cores[0];

                bool incomingIsSub = (core.type == CoreSO.CoreType.sub);
                bool existingIsSub = (existCore != null && existCore.type == CoreSO.CoreType.sub);

                // 서브-서브 금지
                if (incomingIsSub && existingIsSub)
                {
                    Debug.Log("한 슬롯 내에 서브코어 2개는 불가능!");
                    continue; // 다음 슬롯 시도
                }

                cores.Add(core);
                Debug.Log($"슬롯 {i + 1}에 {core.coreName} 추가함.");
                TryCombine(s, i); // 코어가 2개이므로 조합 시도
                return true;
            }
        }
        Debug.Log("모든 슬롯 꽉참!");
        return false;
    }

    // === 조합 시도 ===
    private void TryCombine(Slot s, int slotIndex)
    {
        if (s == null) return;
        if (!s.ReadyToCombine()) return;

        if (s.Cores == null || s.Cores.Count < Slot.Capacity) return;

        BlueprintSO bp = s.blueprint;
        CoreSO a = s.Cores[0];
        CoreSO b = s.Cores[1];

        Debug.Log("무기 조합 시도한다");
        WeaponSO w = weapon.GetWeapon(bp, a, b);
        Debug.Log("여긴 문제 없음");

        if (w != null)
        {
            s.ClearSlot();           // 코어/설계도 비우기
            s.combinedWeapon = w;    // 조합 결과 저장
            HUDManager.Instance.UpdateWeaponSlot(slotIndex, w);
            Debug.Log($"슬롯 {slotIndex + 1}에 {w.weaponName} 추가함.");
        }
        else
        {
            Debug.Log("조합 실패.");
        }
    }
}
