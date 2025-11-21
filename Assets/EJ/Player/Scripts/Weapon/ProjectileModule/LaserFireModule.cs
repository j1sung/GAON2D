using UnityEngine;

public sealed class LaserFireModule : IFireModule
{
    RuntimeWeapon r;
    Transform owner;
    IPool pool;
    ISubCoreModule sub;

    public void Init(FireInitContext ctx)
    {
        r = ctx.runtime;
        owner = ctx.owner;
        pool = ctx.pool;
        sub = ctx.subCore;
        sub?.ApplyAccel(ref r);
    }

    public void Fire(in FireContext ctx)
    {
        var basePlan = new ShotPlan { count = 1, dir = new[] { ctx.aimDir.normalized } }; // 기본 발사 방향
        var plan = sub?.ApplyMultiple(r, ctx, basePlan) ?? basePlan; // 확산 코어 존재시, basePlan을 기준으로 추가 방향 지정

        for (int i = 0; i < plan.count; i++)
        {
            Vector2 dir = plan.dir[i].sqrMagnitude > 0.0001f ? plan.dir[i].normalized : Vector2.right;

            var go = pool.Get(r.bulletPrefabKey, ctx.muzzle.position, Quaternion.identity);
            var beam = go.GetComponent<LaserBeamSprite>();

            beam.Init(new LaserBeamSprite.Args
            {
                owner = owner,
                origin = ctx.muzzle.position,
                dir = dir,
                length = Mathf.Max(0.5f, r.RUN_projectileSpeed),     // 길이
                life = Mathf.Max(0.03f, r.RUN_projectileLifetime), // 반짝 시간
                width = 0.12f,               // 굵기(원하면 SO에 필드 하나 추가)
                damage = r.RUN_damage,
                status = r.RUN_OnHitTags,
                pierceAll = true,                 // 여러 적 관통(원하면 false)
            });
        }
    }
}