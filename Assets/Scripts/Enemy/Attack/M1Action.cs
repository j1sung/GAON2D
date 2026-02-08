using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1Action : MonoBehaviour, IEnemyAction
{
    [SerializeField] private EnemyBulletSpawn spawn;

    private bool _hasPending;
    private AttackValue _pending;
    public void Attack(in AttackValue attackValue)
    {
        // 공격 모드 설정 안됐으면 중단.
        if (attackValue.mode == AttackMode.None)
        {
            _hasPending = false;
            return;
        }

        // 발사 예약
        _pending = attackValue;
        _hasPending = true;

        //// 일반 공격 - 원거리 공격 실행!
        //else if (attackValue.mode == AttackMode.Normal)
        //{
        //    // 총탄 발사!
        //    spawn.Shoot(attackValue);
        //}
    }

    // Attack 애니메이션의 "발사 프레임"에 이벤트로 호출
    public void AnimEvent_Shoot()
    {
        if (!_hasPending) return;

        // Normal 공격만 총알 (스킬이면 다른 처리)
        if (_pending.mode == AttackMode.Normal)
            spawn.Shoot(_pending);

        _hasPending = false; // 1회 발사 후 초기화
    }
}