using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float chaseRange = 7f; // 기존 Enemy의 chaseRange

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Rigidbody2D target;

    // Patrol
    private bool goingLeft = true;
    private float leftX;
    private float rightX;

    private EnemyStatsController status; // Enemy에서 주입

    public void Init(EnemyStatsController stats, Rigidbody2D rb, SpriteRenderer sr)
    {
        status = stats;
        rigid = rb;
        spriter = sr;
    }

    public void SetTarget(Rigidbody2D t) => target = t;

    // ===== Patrol =====
    public void ReAllocCurrentPos(Vector3 pos)
    {
        leftX = pos.x - 2f;
        rightX = pos.x + 2f;
    }

    public void PatrolAround()
    {
        float targetX = goingLeft ? leftX : rightX;
        float dir = Mathf.Sign(targetX - transform.position.x);

        Vector2 nextPos = new Vector2(
            transform.position.x + dir * status.Speed * Time.fixedDeltaTime,
            transform.position.y
        );

        rigid.MovePosition(nextPos);

        // 바라보는 방향 변경
        spriter.flipX = goingLeft;

        if (Mathf.Abs(transform.position.x - targetX) < 0.05f)
            goingLeft = !goingLeft;
    }

    // ===== Chase =====
    public bool IsInChaseRange()
    {
        if (target == null) return false;
        return Vector2.Distance(transform.position, target.position) <= chaseRange;
    }

    public void MoveToTarget()
    {
        if (target == null) return;

        // 적 -> 플레이어 방향 = 위치차이 정규화
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * status.Speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    public bool IsInAttackRange(float attackRange)
    {
        if (target == null) return false;

        // 적과 플레이어 사이 거리가 공격 사거리 범위 내부라면 true
        return Vector2.Distance(transform.position, target.position) <= attackRange;
    }

    // 디버그용 (Enemy가 Gizmo 그릴 때 접근 가능하게)
    public float ChaseRange => chaseRange;
    public Rigidbody2D Target => target;

    private Coroutine dashRoutine;

    public void DashTo(Vector2 targetPos, float speed, float duration)
    {
        if (dashRoutine != null)
            StopCoroutine(dashRoutine);

        dashRoutine = StartCoroutine(DashCoroutine(targetPos, speed, duration));
    }
    public void StopDash()
    {
        if (dashRoutine != null)
        {
            StopCoroutine(dashRoutine);
            dashRoutine = null;
        }
        rigid.velocity = Vector2.zero; // 멈춤
    }

    private IEnumerator DashCoroutine(Vector2 targetPos, float speed, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.fixedDeltaTime;

            Vector2 pos = rigid.position;
            Vector2 to = targetPos - pos;

            if (to.sqrMagnitude < 0.1f)
                break;

            Vector2 step = to.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(pos + step);
            rigid.velocity = Vector2.zero; // Rigidbody2D의 기존 속도가 영향을 주지 않게 강제로 속도 없애기.

            spriter.flipX = targetPos.x < pos.x;

            yield return new WaitForFixedUpdate();
        }

        rigid.velocity = Vector2.zero;
    }
}
