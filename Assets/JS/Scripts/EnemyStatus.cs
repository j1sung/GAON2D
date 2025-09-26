using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float Speed { get; private set; }
    public float AttackRange { get; private set; }

    // Status 셋팅
    public void InitStatus(EnemyData data)
    {
        MaxHealth = data.health;
        Speed = data.speed;
        AttackRange = data.attackRange;
    }

    public void ResetStatus()
    {
        CurrentHealth = MaxHealth;
    }

    // Status 기능들
    public bool IsDead => CurrentHealth <= 0;

    public void ReduceHealth(float amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);
    }
}
