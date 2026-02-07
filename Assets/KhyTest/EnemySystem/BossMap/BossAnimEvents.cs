using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimEvents : MonoBehaviour
{
    [SerializeField] private EnemyState enemyState;

    private void Awake()
    {
        if (enemyState == null)
        {
            enemyState = GetComponentInParent<EnemyState>();
        }
    }

    public void AE_AttackHit()
    {
        // 플레이어 타격 판정.
        enemyState.AttackHit();
    }

    public void AttackEnd()
    {
        enemyState.AttackEnd();
    }
}
