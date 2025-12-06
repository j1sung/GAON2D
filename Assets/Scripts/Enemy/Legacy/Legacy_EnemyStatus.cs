using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legacy_EnemyStatus : MonoBehaviour
{
    public float MaxHealth { get; private set; }
    [field: SerializeField] public float CurrentHealth { get; private set; }
    public float Speed { get; private set; }
    public float AttackRange { get; private set; }
    public float Damage {  get; private set; }

    // Status 셋팅
    public void InitStatus(EnemyData data)
    {
        MaxHealth = data.health;
        Speed = data.speed;
        AttackRange = data.attackRange;
        Damage = data.damage;
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


// 적 기본 값 확인
/*
 * 1. ScriptableObject로 적 기본 스탯 관리
 * 2. EnemyStats 클래스 생성 (MaxHealth, Speed, AttackRange)
 * 3. EnemyStats 상태 관리 클래스 생성 (현재 체력, 데미지 처리)
 * 
 * 
 public class EnemyStats{
    public float MaxHelath;
    public float Speed;
    public float AttackRange;

    public EnemyStats(float maxHealth, float speed, float attackRange){
        MaxHelath = maxHealth;
        Speed = speed;
        AttackRange = attackRange;
    }

    public EnemtStats Clone(){
        return new EnemyStats(MaxHelath, Speed, AttackRange);
    }
}
 
public class a : MonoBehaviour{
    [SerializeField] private EnemyStats baseStats;
    public EnemyStats RuntimeStats { get; private set; }

    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

   private void Awake(){
        RuntimeStats = baseStats.Clone();
        CurrentHealth = RuntimeStats.MaxHelath;
    }
    
    public void TakeDamage(float amount){
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);
    }

    public void ResetStats() {
    CurrentHealth = RuntimeStats.MaxHelath;
    }
}
 */