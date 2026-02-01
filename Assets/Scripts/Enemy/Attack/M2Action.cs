using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2Action : MonoBehaviour, IEnemyAction
{
    [SerializeField] private float dashSpeed = 1f; // 대쉬 스피드
    [SerializeField] private float dashDuration = 2f; // 대쉬 동작 시간

    private EnemyMovement movement;
    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
    }

    public void Attack(in AttackValue attackValue)
    {
        // 공격 모드 설정 안됐으면 중단.
        if (attackValue.mode == AttackMode.None)
        {
            movement.StopDash();
            return;
        } 

        // 일반 공격
        else if(attackValue.mode == AttackMode.Normal)
        {
            // 돌진 실행!
            movement.DashTo(attackValue.targetPos, dashSpeed, dashDuration);
        }
           
    }
}
