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
            }
        }
        // 설계도에 넣을 자리가 없음
        Debug.Log("설계도 넣을 자리 없음");
        return false;
    }

    // === 코어 줍기 ===
    public bool PickupCore(CoreSO core)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Slot s = slots[i];
            if (s.CanAddCore())
            {
                s.Cores.Add(core);
                Debug.Log("슬롯" + (i + 1) + "에" + core.coreName + "추가함.");

                if (s.ReadyToCombine())
                {
                    BlueprintSO bp = s.blueprint;
                    CoreSO a = s.Cores[0];
                    CoreSO b = s.Cores[1];
                    WeaponSO w = weapon.GetWeapon(bp, a, b);

                    if (w != null)
                    {
                        s.ClearSlot();
                        s.combinedWeapon = w;
                        Debug.Log("슬롯" + (i + 1) + "에" + w.weaponName + "추가함.");
                    }
                    else
                        Debug.Log("조합 실패.");
                }
            }
        }
        Debug.Log("모든 슬롯 꽉참!");
        return false;
    }
}
