using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class M1Action : MonoBehaviour, IEnemyAction
{
    private Transform owner;
    private float currentDamage;
    private StatusTag status;

    public void Attack(Transform transform, float damage)
    {
        owner = transform;
        currentDamage = damage;

        Debug.Log("M1 박치기 공격!");
        // 공격 애니메이션 넣기
        // 애니메이션 트리거 감지

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //if (dead) return;

        var dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            var hit = new HitContext
            {
                attacker = owner,
                damage = currentDamage,
                statusTags = status
            };
            Debug.Log("적 공격 데미지: "+currentDamage);
            dmg.ApplyHit(hit);
        }
    }
}