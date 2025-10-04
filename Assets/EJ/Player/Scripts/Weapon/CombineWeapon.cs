/// 무기의 Scriptable Object를 바탕으로 조합을 시도하는 코드이다.
/// 게임 시작시, Resources/WeaponData 폴더 내에 있는 모든 무기 데이터를 로드한다.
/// 모든 무기의 데이터를 딕셔너리를 통해 관리한다.

using System;
using System.Collections.Generic;
using UnityEngine;

public class CombineWeapon : MonoBehaviour
{
    [Header("무기 데이터")]
    [SerializeField] private List<WeaponSO> allWeapons = new();
    [SerializeField] private string resourcesPath = "WeaponData"; // Assets/Resources/WeaponData

    private Dictionary<(string, string, string), WeaponSO> table; // 조합식을 Key로, 무기 SO를 Value로 딕셔너리 생성

    // 게임 시작시 무기 데이터를 바로 로드
    void Awake()
    {
        if ((allWeapons == null || allWeapons.Count == 0) && !string.IsNullOrEmpty(resourcesPath))
            allWeapons = new List<WeaponSO>(Resources.LoadAll<WeaponSO>(resourcesPath));

        BuildTable();
    }

     private void BuildTable()
    {
        table = new Dictionary<(string, string, string), WeaponSO>(allWeapons.Count);

        foreach (var w in allWeapons)
        {
            if (w == null || w.blueprint == null || w.coreA == null || w.coreB == null) continue;

            var bp = w.blueprint.code;
            var a  = w.coreA.code;
            var b  = w.coreB.code;
            if (string.IsNullOrEmpty(bp) || string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) continue;

            var (low, high) = SortASC(a, b);     // 코어 순서 무시
            table[(bp, low, high)] = w;                 // 같은 키면 마지막 항목이 남음
        }
    }

    public WeaponSO GetWeapon(BlueprintSO bp, CoreSO a, CoreSO b)
    {
        if (bp == null || a == null || b == null) return null;
        if (string.IsNullOrEmpty(bp.code) || string.IsNullOrEmpty(a.code) || string.IsNullOrEmpty(b.code)) return null;

        var (low, high) = SortASC(a.code, b.code);
        
        if (table.TryGetValue((bp.code, low, high), out WeaponSO w))
        {
            return w;
        }
        return null;
    }

    // 코드 문자열을 사전식으로 오름차순 정렬하여 (low, high) 반환
    private static (string low, string high) SortASC(string a, string b)
    {
        int cmp = string.CompareOrdinal(a, b);
        return (cmp <= 0) ? (a, b) : (b, a);
    }
}