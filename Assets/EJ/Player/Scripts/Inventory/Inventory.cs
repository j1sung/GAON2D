/// 습득한 설계도/코어를 슬롯에 추가하는 클래스이다.
/// 슬롯의 자리 여부를 판단해서 슬롯0부터 차례대로 채운다.
/// 설계도나 코어를 획득 할 때마다 해당 슬롯에서 무기 조합을 시도한다.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int slotCount = 4;
    public Slot[] slots;

    public CombineWeapon weapon;

    [Header("Default Weapon")]
    public WeaponSO defaultWeapon;

    // 조합 성공 알림(WeaponManager가 구독해서 컨트롤러 재구성)
    public event Action OnWeaponsChanged;

    // 선택된 조합무기 슬롯 인덱스
    private int selectedSlotIndex = -1;

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
                TryCombine(s, i);
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

            var cores = s.Cores ??= new List<CoreSO>(Slot.Capacity); // Cores가 Null 이면 새로 리스트 생성
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

        WeaponSO w = weapon.GetWeapon(bp, a, b);

        if (w != null)
        {
            s.ClearSlot();           // 코어/설계도 비우기
            s.combinedWeapon = w;    // 조합 결과 저장
            HUDManager.Instance.UpdateWeaponSlot(slotIndex, w);
            Debug.Log($"슬롯 {slotIndex + 1}에 {w.weaponName} 추가함.");

            // 만약 첫 조합이면, 자동으로 선택
            if (selectedSlotIndex == -1)
            {
                selectedSlotIndex = slotIndex;
                Debug.Log($"첫 조합무기 자동 선택: 슬롯 {slotIndex + 1}");
            }
            OnWeaponsChanged?.Invoke(); // 무기 변경 이벤트 발행
        }
        else
        {
            Debug.Log("조합 실패.");
        }
    }

    // 현재 선택된 조합무기를 직접 선택하는 함수
    public bool SelectCombinedWeapon(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.Log("잘못된 슬롯 인덱스");
            return false;
        }

        if (slots[slotIndex].combinedWeapon == null)
        {
            Debug.Log("선택할 무기가 없습니다.");
            return false;
        }

        selectedSlotIndex = slotIndex;
        Debug.Log($"조합 무기 선택: 슬롯 {slotIndex + 1}");
        OnWeaponsChanged?.Invoke(); // 무기 변경 알림
        return true;
    }

    // 현재 선택된 조합 무기 반환 (없으면 null)
    public WeaponSO GetSelectedCombinedWeapon()
    {
        if (selectedSlotIndex < 0 || selectedSlotIndex >= slots.Length)
            return null;

        return slots[selectedSlotIndex].combinedWeapon;
    }

    // 현재 전투에서 실제로 사용할 무기 목록 반환 (기본무기 + 선택된 조합무기 1개)
    public List<WeaponSO> GetActiveWeapons()
    {
        var list = new List<WeaponSO>(2);

        // 기본무기 
        if (defaultWeapon != null)
            list.Add(defaultWeapon);

        // 선택된 조합무기
        WeaponSO selected = GetSelectedCombinedWeapon();
        if (selected != null)
            list.Add(selected);

        return list;
    }
}
