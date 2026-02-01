using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsController
{
    //public EnemyStats baseStats { get; private set; }
    private EnemyData source; // SO ����
    public EnemyStats RuntimeStats { get; private set; }
    public float CurrentHealth { get; private set; }

    // Get ���ٿ� ������Ƽ
    public float Speed => RuntimeStats.Speed;
    public float NormalDamage => RuntimeStats.NormalDamage;
    public float SkillDamage => RuntimeStats.SkillDamage;
    public float AttackRange => RuntimeStats.AttackRange;
    public float MaxHealth => RuntimeStats.MaxHelath;

    public bool IsDead => CurrentHealth <= 0;
    public EnemyStatsController(EnemyData data)
    {
        source = data;
    }
    public void ResetStats()
    {
        RuntimeStats = new EnemyStats(source.health, source.speed, source.attackRange, source.normalDamage, source.skillDamage);
        CurrentHealth = RuntimeStats.MaxHelath;
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);
    }
}
