using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    public float MaxHelath;
    public float Speed;
    public float AttackRange;
    public float NormalDamage;
    public float SkillDamage;

    public EnemyStats(float maxHealth, float speed, float attackRange, float normalDamage, float skillDamage)
    {
        MaxHelath = maxHealth;
        Speed = speed;
        AttackRange = attackRange;
        NormalDamage = normalDamage;
        SkillDamage = skillDamage;
    }

    public EnemyStats Clone()
    {
        return new EnemyStats(MaxHelath, Speed, AttackRange, NormalDamage, SkillDamage);
    }
}