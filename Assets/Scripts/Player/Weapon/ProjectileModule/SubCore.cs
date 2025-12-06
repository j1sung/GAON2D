using UnityEngine;

public sealed class AccelCore : ISubCoreModule
{
    [Tooltip("발사 간격 배수 (0.8이면 20% 빨라짐)")]
    public float intervalMultiplier = 0.8f;

    // 가속: 발사 간격(RUN_fireRate)을 줄여 연사속도를 높인다.
    // Init 시점(1회)만 적용됨.
    public void ApplyAccel(ref RuntimeWeapon r)
    {
        // 최소 간격 안전장치
        r.RUN_fireRate = Mathf.Max(0.01f, r.RUN_fireRate * intervalMultiplier);
    }

    // 가속 코어는 발사 패턴을 바꾸지 않음. 기본 플랜 그대로 반환.
    public ShotPlan ApplyMultiple(in RuntimeWeapon r, in FireContext ctx, ShotPlan basePlan)
    {
        return basePlan;
    }
    // public void MutatePerShot(ref ShotStats s, int shotIndex, in ShotPlan plan) { }
}

public sealed class SpreadCore : ISubCoreModule
{
    [Header("확산 파라미터")]
    [Tooltip("추가 발사 수 (기본 1발 + extraShots)")]
    public int   extraShots = 2;

    [Tooltip("인접 샷 간 각도 간격(도)")]
    public float spreadDeg  = 10f;

    public void ApplyAccel(ref RuntimeWeapon r) { }

    /// 확산: 이번 발사 틱에서 여러 발을 생성하고 각도를 분배한다.
    /// 중심 정렬: -(n-1)/2 * spreadDeg ~ +(n-1)/2 * spreadDeg
    public ShotPlan ApplyMultiple(in RuntimeWeapon r, in FireContext ctx, ShotPlan basePlan)
    {
        int total = Mathf.Max(1, basePlan.count + extraShots);
        var plan  = new ShotPlan { count = total, dir = new Vector2[total] };

        // 중심 기준 좌/우 대칭 분배
        float center = (total - 1) * 0.5f;
        Vector2 aim = basePlan.dir != null && basePlan.dir.Length > 0
                        ? basePlan.dir[0].sqrMagnitude > 0.0001f ? basePlan.dir[0].normalized
                                                                  : ctx.aimDir.normalized
                        : ctx.aimDir.normalized;

        for (int i = 0; i < total; i++)
        {
            float angle = (i - center) * spreadDeg; // -k*deg ... 0 ... +k*deg
            plan.dir[i] = (Quaternion.Euler(0, 0, angle) * aim).normalized;
        }
        return plan;
    }
    // public void MutatePerShot(ref ShotStats s, int shotIndex, in ShotPlan plan) { }
}