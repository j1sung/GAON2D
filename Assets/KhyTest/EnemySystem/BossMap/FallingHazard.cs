//using System;
//using UnityEngine;

///*
// * 스폰될때 이미 타켓 위치 시작.
// * N초 동안 애니메이션만 재생. -> 지금은 7초.
// * 애니메이션 끝나는 프레임에서 판정
// * 
// * N초 후 실제 피격 판정 시작.
// * 재생 후 풀로 반납.
// */

//public class FallingHazard : MonoBehaviour
//{
//    [SerializeField] private LayerMask targetMask;
//    [SerializeField] private float radius = 0.6f;
//    [SerializeField] private Transform hitPoint;


//    [Header("Movement")]
//    //[SerializeField] private float fallSpeed = 12f;
//    //[SerializeField] private float xFollowSpeed = 20f;

//    [Header("Life")]
//    [SerializeField] private float activeDuration = 2.0f;
//    [SerializeField] private Animator animator;
//    [SerializeField] private string playStateName = "Play";


//    [SerializeField] private float lifeTime = 5f;


//    private bool damageEnabled;

//    private Vector2 targetPos;
//    private bool active;
//    private float timer;

//    //------------------------
//    [SerializeField] private Transform hitArea;
//    [SerializeField] private Collider2D hitCollider;
//    [SerializeField] private float hitWindow = 0.05f;

//    private Vector2 lockedPos;
//    private Action<FallingHazard> release;
//    //------------------------


//    public void Spawn(Vector2 targetPos, Action<FallingHazard> releaseCallback)
//    {
//        /*this.targetPos = targetPos;
//        release = releaseCallback;

//        timer = 0f;
//        active = true;
//        gameObject.SetActive(true);*/
//        /*transform.position = targetPos;

//        release = releaseCallback;
//        timer = 0f;
//        active = true;
//        gameObject.SetActive(true);

//        if (animator != null && !string.IsNullOrEmpty(playStateName))
//            animator.Play(playStateName, 0, 0f);
//        */

//        release = releaseCallback;
//        active = true;
//        damageEnabled = false;
//        timer = 0f;

//        // 스폰 순간에 플레이어 위치 잠금.
//        lockedPos = targetPos;

//        // hitArea 위치 고정 .
//        if (hitArea != null) hitArea.transform.position = lockedPos;

//        // 애니메이션은 실행되지만, 판정은 따로.
//        gameObject.SetActive(true);

//        if (hitCollider != null) hitCollider.enabled = false;

//        animator.Play(playStateName, 0, 0f);
//    }

//    private void Update()
//    {
//        if (!active) return;

//        /*Vector2 pos = transform.position;

//        pos.x = Mathf.Lerp(pos.x, targetPos.x, Time.deltaTime * xFollowSpeed);
//        pos.y -= fallSpeed * Time.deltaTime;
//        transform.position = pos;

//        if (pos.y <= targetPos.y)
//        {
//            Despawn();
//            return;
//        }*/

//        timer += Time.deltaTime;
//        if (timer >= lifeTime)
//        {
//            Despawn();
//            //return;
//        }

//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        /*var dmg = other.GetComponentInParent<IDamageable>();
//        if (dmg != null)
//        {
//            dmg.ApplyHit(new HitContext
//            {
//                attacker = transform,
//                damage = 10f,
//                statusTags = default
//            });
//        }

//        Despawn();
//        */
//        if (!damageEnabled) return;

//        var dmg = other.GetComponentInParent<IDamageable>();
//        if (dmg != null)
//        {
//            dmg.ApplyHit(new HitContext
//            {
//                attacker = transform,
//                damage = 10f,
//                statusTags = default
//            });
//        }

//        Despawn();
//    }

//    private void Despawn()
//    {
//        if (!active) return;
//        active = false;

//        if (release != null) release(this);
//        else gameObject.SetActive(false);
//    }

//    public void EnableDamage()
//    {
//        if (!active) return;
//        StartCoroutine(HitOnce());
//    }

//    private System.Collections.IEnumerator HitOnce()
//    {
//        damageEnabled = true;
//        if (hitCollider != null) hitCollider.enabled = true;

//        yield return new WaitForSeconds(hitWindow);

//        if (hitCollider != null) hitCollider.enabled = false;
//        damageEnabled = false;

//        Despawn();
//    }
//}

using System;
using UnityEngine;

public class FallingHazard : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform impact;                 // Impact 오브젝트(기둥)
    [SerializeField] private Animator impactAnimator;          // Impact Animator
    [SerializeField] private string playStateName = "Play";

    [SerializeField] private Transform telegraph;              // Telegraph 오브젝트(원)
    [SerializeField] private SpriteRenderer telegraphRenderer; // 원 스프라이트
    [SerializeField] private Collider2D hitCollider;           // Telegraph의 BoxCollider2D

    [Header("Hit")]
    [SerializeField] private LayerMask targetMask;             // Player 레이어 포함
    [SerializeField] private float damage = 10f;

    [Header("Timing")]
    [SerializeField] private float spawnHeight = 8f;           // 위에서 시작 높이
    [SerializeField] private float lifeTime = 10f;             // 혹시 모를 안전 타임아웃

    private bool active;
    private float timer;
    private Vector2 lockedPos;
    private Action<FallingHazard> release;

    public void Spawn(Vector2 playerPos, Action<FallingHazard> releaseCallback)
    {
        release = releaseCallback;
        active = true;
        timer = 0f;

        lockedPos = playerPos;

        if (telegraph != null) telegraph.position = lockedPos;
        if (telegraphRenderer != null) telegraphRenderer.enabled = true;

        if (hitCollider != null) hitCollider.enabled = false;

        if (impact != null) impact.position = lockedPos + Vector2.up * spawnHeight;

        gameObject.SetActive(true);
        if (impactAnimator != null && !string.IsNullOrEmpty(playStateName))
            impactAnimator.Play(playStateName, 0, 0f);
    }

    private void Update()
    {
        if (!active) return;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
            Despawn();
    }

    public void EnableDamage()
    {
        if (!active) return;
        if (hitCollider == null) { Despawn(); return; }

        var center = hitCollider.bounds.center;
        var size = hitCollider.bounds.size;

        var hits = Physics2D.OverlapBoxAll(center, size, 0f, targetMask);

        for (int i = 0; i < hits.Length; i++)
        {
            var dmgTarget = hits[i].GetComponentInParent<IDamageable>();
            if (dmgTarget != null)
            {
                dmgTarget.ApplyHit(new HitContext
                {
                    attacker = transform,
                    damage = damage,
                    statusTags = default
                });
            }
        }

        Despawn();
    }

    private void Despawn()
    {
        if (!active) return;
        active = false;

        if (telegraphRenderer != null) telegraphRenderer.enabled = false;
        if (hitCollider != null) hitCollider.enabled = false;

        release?.Invoke(this);
        if (release == null) gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    // Scene에서 원 범위 디버그로 보기
    private void OnDrawGizmosSelected()
    {
        if (hitCollider == null) return;
        Gizmos.DrawWireCube(hitCollider.bounds.center, hitCollider.bounds.size);
    }
#endif
}

