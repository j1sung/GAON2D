using System;
using System.Collections;
using UnityEngine;

public class FallingHazard : MonoBehaviour
{
    /*
    1. 플레이어 위치 샘플링.
    2. 바닥에 경고 원 생성.
    - 원이 서서히 커지거나 채워지는 연출.
    3. 딜레이 동안.
    3.1 원이 플레이어를 따라다니게 할지, 처음 위치에 고정할지 결정.
    4. 딜레이가 끝나면.
    원 제거 후 컨테이너를 그 x,y 좌표의 위쪽에서 스폰.
    그리고 아래로 이동 (음... 애니메이션, 트윈, 리지드바디).
    5. 바닥에 도달하면.
    충돌 처리, 이펙트, 데미지 판정.
    오브젝트는 풀로 반환.
     */

    // -----------------------------------------------------------------
    // 애니메이션으로 대체 하므로 추가.
    [Header("Animatior")]
    [SerializeField] private Animator animator;
    [SerializeField] private string playTrigger = "Play";
    [SerializeField] private string idelStateName = "Idle";

    [Header("Damage")]
    [SerializeField] private Collider2D hitArea;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private int damage = 1;
    // -----------------------------------------------------------------

    [Header("Drop")]
    [SerializeField] private float fallTime = 0.4f;

    [Header("Life Safety")]
    // 애니메이션 시간보다 살짝 여유있게.
    [SerializeField] private float lifeTime = 15f; // 혹시 버그 나면 자동 반납

    private bool active;
    private float timer;

    private bool damageEnabled;
    private bool hasHit;

    private Action<FallingHazard> release;
    private Coroutine running;

    // BossMapPatternController에서 호출할 메서드
    // 애니메이션 재생이므로 스폰 지점에 최종 targetPos로.
    // 오브젝트를 바로 옮겨두고 애니메이션 재생.
    public void Spawn(Vector2 targetPos, Action<FallingHazard> releaseCallback)
    {
        release = releaseCallback;
        active = true;
        timer = 0f;

        // 최종 히트 위치에 "고정" 배치 (애니메이션 실행).
        //transform.position = targetPos;

        // HitArea 기준으로 targetPos에 맞추기.
        AlignHitAreaTo(targetPos);

        // 데미지는 기본 off (6초 이벤트 On).
        if (hitArea != null) hitArea.enabled = false;

        if (animator != null)
        {
            animator.ResetTrigger(playTrigger);
            animator.SetTrigger(playTrigger);
        }
        else
        {
            Despawn();
        }


        // 중복 코루틴 방지
        //if (running != null) StopCoroutine(running);
        //running = StartCoroutine(CoFall(targetPos));
    }

    // 애니메이션 실행하면 더 이상 위치 보간 및 낙하 계산 안해도 됨.

    private IEnumerator CoFall(Vector2 targetPos)
    {
        Vector2 start = transform.position;

        float t = 0f;
        while (t < fallTime)
        {
            t += Time.deltaTime;
            //float p = Mathf.Clamp01(t / fallTime);
        
            // 실제 움직이는 부분.
            // 애니메이션 부분으로 변경예정.

            // 낙하 느낌 (ease-in)
            //float eased = p * p;
        
            //transform.position = Vector2.Lerp(start, targetPos, eased);
            yield return null;
        }
        
        transform.position = targetPos;

        // TODO: 여기서 이펙트/데미지/카메라쉐이크 등 처리 가능

        Despawn();
    }

    private void Update()
    {
        if (!active) return;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Despawn();
        }
    
    }

    // ===== Animation Events =====

    // <summary>
    //  애니메이션 6초 지점에 AnimationEvent로 호출.
    // </summary>
    // 애니메이션 6초 지점에 이벤트로 호출.
    public void EnableDamage()
    {
        if (!active) return;

        if (hitArea != null) hitArea.enabled = true;

        // 여기서 "한 번만" 판정하고 싶으면 Overlap으로 바로 처리 추천.
        DoDamageOnce();

    }

    // <summary>
    //  데미지 프레임 끝나는 지점에 AnimationEvent로 호출.
    // 또는 EnbaleDamage에서 1회 판정 후 바로 끄고 싶으면 자동 호출.
    // </summary>
    // 애니메이션에서 데미지 프레임이 끝나는 지점에 이벤트로 호출 (선택)
    public void DisableDamage()
    {
        if (hitArea != null) hitArea.enabled = false;
    }

    /// <summary>
    /// 애니메이션 마지막(11초 끝)에 AnimationEvent로 호출
    /// </summary>
    // 애니메이션 마지막(11초 끝)에 이벤트로 호출
    public void OnAnimFinished()
    {
        Despawn();
    }

    private void DoDamageOnce()
    {
        if (hitArea == null) return;

        // hitArea의 바운즈로 OverlapBox 판정(Trigger 여부 상관없이 "한 번" 체크 가능)
        Vector2 center = hitArea.bounds.center;
        Vector2 size = hitArea.bounds.size;

        Collider2D[] cols = Physics2D.OverlapBoxAll(center, size, 0f, targetMask);
        if (cols == null || cols.Length == 0) return;

        for (int i = 0; i < cols.Length; i++)
        {
            var col = cols[i];
            if (col == null) continue;

            var damageable = col.GetComponent<IDamageable>();
            if (damageable == null) continue;

            // ApplyHit(HitContext)
            HitContext ctx = new HitContext
            {
                damage = damage,
            };

            damageable.ApplyHit(ctx);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!damageEnabled) return;

        var dmg = other.GetComponentInParent<IDamageable>();
        if (dmg != null)
        {
            dmg.ApplyHit(new HitContext
            {
                damage = 10f,
            });
        }

        Despawn();
    }

    private void Despawn()
    {
        if (!active) return;
        active = false;

        //if (running != null)
        //{
        //    StopCoroutine(running);
        //    running = null;
        //}
        if (hitArea != null)
        {
            hitArea.enabled = false;
        }
        
        // 풀로 반환
        if (release != null) release(this);
        else gameObject.SetActive(false);
    }

    public void AlignHitAreaTo(Vector2 targetPos)
    {
        // hitArea
        if (hitArea == null) return;

        Vector3 delta = (Vector3)targetPos - hitArea.transform.position;
        transform.position += delta; 
    }

#if UNITY_EDITOR
    // hitArea OverlapBox 범위
    private void OnDrawGizmosSelected()
    {
        if (hitArea == null) return;

        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawWireCube(hitArea.bounds.center, hitArea.bounds.size);
    }
#endif
}

