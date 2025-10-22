using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Spawn/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float spawnTime;

    public string enemyName;
    public RuntimeAnimatorController controller;
    public int health;
    public float speed;
    public float attackRange;
}

[CreateAssetMenu(fileName = "BossData", menuName = "Spawn/BossData")]
public class BossData : EnemyData
{
    public int phaseCount;
    public float ultimateCooldown;
}