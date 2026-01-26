using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossEnemyStats", menuName = "Enemy/Stats/Boss Enemy Stats", order = 1)]
public class BossEnemyStats : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHp = 100f;

    [SerializeField] private float attackPower = 10f;

    // 외부에서 읽기 전용
    public float AttackPower => attackPower;
}
