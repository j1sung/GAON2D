using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsController
{
    public EnemyStats baseStats { get; private set; }
    public EnemyStats RuntimeStats { get; private set; }
    public float CurrentHealth { get; private set; }

    // 단축 접근용 프로퍼티
    public float Speed => RuntimeStats.Speed;
    public float Damage => RuntimeStats.Damage;
    public float AttackRange => RuntimeStats.AttackRange;
    public float MaxHealth => RuntimeStats.MaxHelath;

    public bool IsDead => CurrentHealth <= 0;
    public EnemyStatsController(EnemyData data)
    {
        baseStats = new EnemyStats(data.health, data.speed, data.attackRange, data.damage);
        RuntimeStats = baseStats.Clone();
        //CurrentHealth = RuntimeStats.MaxHelath;
    }
    public void ResetStats()
    {
        CurrentHealth = RuntimeStats.MaxHelath;
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(CurrentHealth);
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);
    }

    
}
