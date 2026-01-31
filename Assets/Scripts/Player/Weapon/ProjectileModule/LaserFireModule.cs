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
        var basePlan = new ShotPlan
        {
            count = 1,
            dir = new[] { ctx.aimDir.normalized }
        };

        var plan = sub?.ApplyMultiple(r, ctx, basePlan) ?? basePlan;

        // 최종 데미지 계산
        float finalDamage = r.RUN_damage;
        if (owner.TryGetComponent<PlayerStatus>(out var status))
        {
            finalDamage = status.GetFinalDamage(finalDamage);
        }

        for (int i = 0; i < plan.count; i++)
        {
            Vector2 dir =
                plan.dir[i].sqrMagnitude > 0.0001f
                    ? plan.dir[i].normalized
                    : Vector2.right;

            var go = pool.Get(r.bulletPrefabKey, ctx.muzzle.position, Quaternion.identity);
            var beam = go.GetComponent<LaserBeamSprite>();

            beam.Init(new LaserBeamSprite.Args
            {
                owner = owner,
                origin = ctx.muzzle.position,
                dir = dir,
                length = Mathf.Max(0.5f, r.RUN_projectileSpeed),
                life = Mathf.Max(0.03f, r.RUN_projectileLifetime),
                width = 0.12f,
                damage = finalDamage,             
                status = r.RUN_OnHitTags,
                pierceAll = true,
            });
        }
    }
}