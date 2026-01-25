using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1Action : MonoBehaviour, IEnemyAction
{
    [SerializeField] private float currentDamage;
    private StatusTag status = 0;

    public void Attack(in AttackValue attackValue)
    {
        currentDamage = attackValue.damage;

        //Debug.Log("M1 박치기 공격!");
        // 공격 애니메이션 넣기
        // 애니메이션 트리거 감지

    }

    // 이제 적의 유형에 따라 몸통 부딪힘 데미지가 없을 수 있어서 다르게 구현되어야함
    void OnTriggerEnter2D(Collider2D other)
    {
        //if (dead) return;
        // 공격 상태가 아니면 트리거 무시
        if (currentDamage <= 0)
            return;

        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            // 지금 구조체로 결국 2번 넘기는거라 만약 같은걸 넘길거면 하나로 통합하는거도 괜찮을듯
            HitContext hit = new HitContext
            {
                damage = currentDamage,
                statusTags = status
            };
            dmg.ApplyHit(hit);
        }
    }
}