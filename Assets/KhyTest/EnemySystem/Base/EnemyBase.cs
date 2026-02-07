using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IEnemy
{
    [Header("Config")]
    [SerializeField] protected BossEnemyStats stats;
    [SerializeField] protected EnemyTier tier = EnemyTier.Normal;

    public float AttackPower => (stats != null) ? stats.AttackPower : 0f;
    public float MaxHP => (stats != null) ? stats.maxHp : 0f;
    public EnemyTier Tier => tier;

    public float CurrentHp { 
        get; 
        protected set;
    }

    protected virtual void Awake()
    {
        if (stats == null)
        {
            enabled = false;
            return;
        }

        CurrentHp = stats.maxHp;
    }

    public virtual void ApplyHit(HitContext ctx)
    {
        if (stats == null)
            return;
        
        CurrentHp -= ctx.damage;
        CurrentHp = Mathf.Clamp(CurrentHp, 0, stats.maxHp);
        
        if (CurrentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public abstract void Attack();
}
