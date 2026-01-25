using UnityEngine;

public sealed class AccelCore : ISubCoreModule
{  
    [Tooltip("발사 간격 배수 (0.8이면 20% 빨라짐)")]
    public float intervalMultiplier = 0.5f;

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
    // 탄환 수
    public int totalShots = 4;

    // 조준 방향 기준 랜덤 각도 범위
    public float randomAngleRangeDeg = 20f;

    public void ApplyAccel(ref RuntimeWeapon r) { }

    public ShotPlan ApplyMultiple(in RuntimeWeapon r, in FireContext ctx, ShotPlan basePlan)
    {
        int total = Mathf.Max(1, totalShots);
        var plan  = new ShotPlan { count = total, dir = new Vector2[total] };

        // 기준 방향(aim) 결정
        Vector2 aim = ctx.aimDir.normalized;
        if (basePlan.dir != null && basePlan.dir.Length > 0)
        {
            Vector2 d0 = basePlan.dir[0];
            if (d0.sqrMagnitude > 0.0001f)
                aim = d0.normalized;
        }

        // 이번 공격(seed) 기준 랜덤
        var rng = new System.Random(ctx.seed);
        float range = Mathf.Max(0f, randomAngleRangeDeg);

        for (int i = 0; i < total; i++)
        {
            // -1 ~ 1사이 값으로 한정하기 위해 *2.0-1.0
            float angle = (float)(rng.NextDouble() * 2.0 - 1.0) * range;
            plan.dir[i] = (Quaternion.Euler(0, 0, angle) * aim).normalized;
        }

        return plan;
    }
}