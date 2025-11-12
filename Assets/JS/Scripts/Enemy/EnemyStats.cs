using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    public float MaxHelath;
    public float Speed;
    public float AttackRange;
    public float Damage;

    public EnemyStats(float maxHealth, float speed, float attackRange, float damage)
    {
        MaxHelath = maxHealth;
        Speed = speed;
        AttackRange = attackRange;
        Damage = damage;
    }

    public EnemyStats Clone()
    {
        return new EnemyStats(MaxHelath, Speed, AttackRange, Damage);
    }
}