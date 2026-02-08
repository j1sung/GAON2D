using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimEvents : MonoBehaviour
{
    private EnemyState enemyState;
    private BossEnemy boss;

    private void Awake()
    {
        enemyState = GetComponentInParent<EnemyState>();
        boss = GetComponentInParent<BossEnemy>();
    }

    public void AE_AttackHit()
    {
        enemyState.AttackHit();
    }

    public void AttackEnd()
    {
        enemyState.AttackEnd();
    }

    public void AE_DieEnd()
    {
        Debug.Log($"{name}: Boss Anim died.");
        boss.FinalizeDeath();
    }
}
