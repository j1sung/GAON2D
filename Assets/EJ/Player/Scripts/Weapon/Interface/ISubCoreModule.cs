// ISubCoreModule.cs
using UnityEngine;

public interface ISubCoreModule
{
    // 가속: 발사 간격 수정
    void ApplyAccel(ref RuntimeWeapon r);

    // 확산: 이번 틱에서의 샷 개수 및 각도 계산
    ShotPlan ApplyMultiple(in RuntimeWeapon r, in FireContext ctx, ShotPlan basePlan);

    // 관통: 각 탄환의 관통 수 등 보정 - 아직 구현 X
    // void MutatePerShot(ref ShotStats s, int shotIndex, in ShotPlan plan);
}

public struct ShotPlan
{
    public int count;        // 발사할 탄 수
    public Vector2[] dir;    // 각 탄의 방향(정규화)
}

public struct ShotStats
{
    public Vector2 dir;
    public float   speed;
    public float   lifetime;
    // public int     pierce;
}