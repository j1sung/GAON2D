using UnityEngine;

//public abstract class EnemyBase : MonoBehaviour, IEnemy

public class EnemyBase : MonoBehaviour
{
   /* [SerializeField] protected EnemyStats stats;
    [SerializeField] protected EnemyTier tier;

    public float AttackPower => stats.AttackPower;
    public EnemyTier Tier => tier;

    public float CurrentHp { 
        get; 
        protected set;
    }

    protected virtual void Awake()
    {
        if (stats == null)
        {
            Debug.LogError("EnemyStats is not assigned!", this);
        }
        CurrentHp = stats.maxHp;
    }

    public virtual void ApplyHit(HitContext ctx)
    {
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
   */
}
