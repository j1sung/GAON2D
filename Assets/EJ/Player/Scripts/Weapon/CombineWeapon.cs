/// 조합된 무기를 반환하는 코드이다.
/// 모든 무기의 SO를 대조하여, 획득한 설계도와 코어들을 조합하면 나오는 아이템의 SO를 리턴한다.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineWeapon : MonoBehaviour
{
    public List<WeaponSO> allWeapons = new List<WeaponSO>();

    // 1개의 설계도 + 2개의 코어로 무기 찾기
    public WeaponSO GetWeapon(BlueprintSO bp, CoreSO a, CoreSO b)
    {
        for (int i = 0; i < allWeapons.Count; i++)
        {
            WeaponSO w = allWeapons[i];
            if (w == null) continue;
            if (w.blueprint != bp) continue;

            if ((w.coreA == a && w.coreB == b) || (w.coreA == b && w.coreB == a))
                return w;
        }
        return null;
    }
}
