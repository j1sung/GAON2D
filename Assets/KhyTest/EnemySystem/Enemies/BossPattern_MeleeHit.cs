using UnityEngine;

public class BossPattern_MeleeHit : MonoBehaviour, IBossPattern
{
    [SerializeField] private float range = 2.0f;

    public void Execute(EnemyBase boss, Transform target, float attackPower)
    {
        if (boss == null || target == null) return;

        float dist = EnemyUtil.Distance2D(boss.transform, target);
        if (dist > range) return;

        var damageable = target.GetComponentInParent<IDamageable>();
        if (damageable == null) return;

        var ctx = new HitContext
        {
            attacker = boss.transform,
            damage = attackPower,
            statusTags = default
        };

        damageable.ApplyHit(ctx);
    }
}
