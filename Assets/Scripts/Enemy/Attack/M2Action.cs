using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2Action : MonoBehaviour, IEnemyAction
{
    //[SerializeField] private float currentDamage;
    [SerializeField] private float dashSpeed = 1f;
    [SerializeField] private float dashDuration = 2f;

    private EnemyMovement movement;
    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
    }
    public void Attack(in AttackValue attackValue)
    {
        //currentDamage = attackValue.damage;

        // 돌진 실행!
        if (attackValue.damage <= 0f)
        {
            movement.StopDash();
            return;
        }

        movement.DashTo(attackValue.targetPos, dashSpeed, dashDuration);
    }

    // 이제 적의 유형에 따라 몸통 부딪힘 데미지가 없을 수 있어서 다르게 구현되어야함
    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    //if (dead) return;
    //    // 공격 상태가 아니면 트리거 무시
    //    if (currentDamage <= 0) return;

    //    var dmg = other.GetComponent<IDamageable>();
    //    if (dmg != null)
    //    {

    //        var hit = new HitContext
    //        {
    //            damage = currentDamage,
    //            statusTags = status
    //        };
    //        dmg.ApplyHit(hit);
    //    }
    //}
}
