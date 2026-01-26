using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsController
{
    //public EnemyStats baseStats { get; private set; }
    private EnemyData source; // SO 참조
    public EnemyStats RuntimeStats { get; private set; }
    public float CurrentHealth { get; private set; }

    // Get 접근용 프로퍼티
    public float Speed => RuntimeStats.Speed;
    public float Damage => RuntimeStats.Damage;
    public float AttackRange => RuntimeStats.AttackRange;
    public float MaxHealth => RuntimeStats.MaxHelath;

    public bool IsDead => CurrentHealth <= 0;
    public EnemyStatsController(EnemyData data)
    {
        source = data;
        //ResetStats(); -> SpawnState에서도 한번 실행되어서 처음에 겹침
    }
    public void ResetStats()
    {
        RuntimeStats = new EnemyStats(source.health, source.speed, source.attackRange, source.damage);
        CurrentHealth = RuntimeStats.MaxHelath;
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);
    }
}
