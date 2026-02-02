using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletVFX : MonoBehaviour
{
    private EnemyBulletPooling ownerPool;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetOwnerPool(EnemyBulletPooling pool) => ownerPool = pool;

    void OnEnable()
    {
        // 재사용 시 항상 처음부터 재생
        if (anim != null) anim.Play(0, 0, 0f);
    }

    void Update()
    {
        if (anim == null || ownerPool == null) return;

        var st = anim.GetCurrentAnimatorStateInfo(0);
        // normalizedTime >= 1 이면 1회 재생 완료 (루프면 안 맞으니 루프 꺼두기)
        if (st.normalizedTime >= 1f && !anim.IsInTransition(0))
        {
            ownerPool.Release(gameObject);
        }
    }
}
