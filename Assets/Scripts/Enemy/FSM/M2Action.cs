using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2Action : MonoBehaviour, IEnemyAction
{
    private Transform owner;
    [SerializeField] private float currentDamage;
    private StatusTag status;

    public void Attack(Transform transform, float damage)
    {
        owner = transform;
        currentDamage = damage;

        //Debug.Log("M2 박치기 공격!");
        // 공격 애니메이션 넣기
        // 애니메이션 트리거 감지

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //if (dead) return;
        // 공격 상태가 아니면 트리거 무시
        if (currentDamage <= 0)
            return;

        var dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {

            var hit = new HitContext
            {
                attacker = owner,
                damage = currentDamage,
                statusTags = status
            };
            dmg.ApplyHit(hit);
        }
    }
}
