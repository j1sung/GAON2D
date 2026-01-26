using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Spawn/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Identity")]
    public string enemyId;
    public string displayName;
    public virtual EnemyType Type => EnemyType.Normal;

    [Header("Spawn")]
    public float spawnTimeSeconds;

    [Header("Stats")]
    public int health;
    public float speed;
    public float attackRange;
    public float damage;

    [Header("Visual (optional)")]
    public RuntimeAnimatorController controller;
}

[CreateAssetMenu(fileName = "BossData", menuName = "Spawn/BossData")]
public class BossData : EnemyData
{
    public override EnemyType Type => EnemyType.Boss;

    public int phaseCount;
    public float ultimateCooldownSeconds;
}