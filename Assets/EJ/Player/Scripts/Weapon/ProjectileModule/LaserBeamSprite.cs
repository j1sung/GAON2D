using UnityEngine;

[RequireComponent(typeof(PoolObject), typeof(SpriteRenderer))]
public sealed class LaserBeamSprite : MonoBehaviour
{
    public struct Args
    {
        public Transform owner;
        public Vector2   origin;
        public Vector2   dir;
        public float     length;       // 레이저 최대 길이
        public float     life;         // 반짝 시간
        public float     width;        // 굵기(스프라이트 Y 스케일)
        public float     damage;       // 단발 피해
        public StatusTag status;       // 상태이상 태그
        public bool      pierceAll;    // 여러 대상 관통 여부
        // public string impactKey;    // 키는 쓰지 않고, 프리팹을 직접 할당해 풀 스폰
    }

    [Header("Hit")]
    [SerializeField] LayerMask hitMask = ~0;

    // 스프라이트 pivot이 center일때를 기준으로 계산
    [SerializeField] bool spritePivotIsCenter = true;

    [Header("Impact VFX (pooled)")]
    [SerializeField] GameObject impactPrefab;

    Transform   owner;
    Vector2     origin;
    Vector2     dir;
    float       reqLength;
    float       life;
    float       width;
    float       damage;
    StatusTag   status;
    bool        pierceAll;

    SpriteRenderer sr;
    PoolObject     po;
    IPool          pool;

    void Awake()
    {
        sr   = GetComponent<SpriteRenderer>();
        po   = GetComponent<PoolObject>();
        pool = Attack.Pooling.PoolManager.instance; // 프로젝트의 풀 매니저 싱글톤
    }

    void OnEnable() { CancelInvoke(); }

    public void Init(in Args a)
    {
        owner     = a.owner;
        origin    = a.origin;
        dir       = (a.dir.sqrMagnitude > 0.0001f) ? a.dir.normalized : Vector2.right;
        reqLength = Mathf.Max(0.5f, a.length);
        life      = Mathf.Max(0.03f, a.life);
        width     = Mathf.Max(0.01f, a.width <= 0 ? 0.1f : a.width);
        damage    = a.damage;
        status    = a.status;
        pierceAll = a.pierceAll;

        // 1) 충돌 처리(임팩트 스폰 포함) + 실제 길이 산출
        float finalLen = DoHitAndComputeLength(origin, dir, reqLength, pierceAll);

        // 2) 시각 배치
        SetVisual(origin, dir, finalLen, width);

        // 3) 반짝 후 사라짐
        Invoke(nameof(Despawn), life);
    }

    void SetVisual(Vector2 o, Vector2 d, float len, float w)
    {
        // 진행 방향으로 회전
        transform.right = d;

        // 스프라이트 본래 크기 기준으로 스케일 계산
        var sprite = sr.sprite;
        float baseX = sprite ? sprite.bounds.size.x : 1f;
        float baseY = sprite ? sprite.bounds.size.y : 1f;

        // 길이(X), 굵기(Y)
        float sx = (baseX > 0.0001f) ? (len / baseX) : len;
        float sy = (baseY > 0.0001f) ? (w   / baseY) : w;
        transform.localScale = new Vector3(sx, sy, 1f);

        // 위치: pivot이 Center면 중앙에, Left면 시작점에 놓기
        transform.position = spritePivotIsCenter ? (o + d * (len * 0.5f)) : (Vector3)o;
    }

    /// <summary>
    /// 충돌 처리(데미지, 임팩트 스폰) 후, 레이저 실제 표시 길이를 반환.
    /// - pierceAll=false: 첫 충돌 지점까지 길이 절단.
    /// - pierceAll=true : 전부 타격(길이는 요청 길이 유지).
    /// </summary>
    bool IsSelf(in RaycastHit2D h)
    {
        if (!h.collider || !owner) return false;
        var t = h.collider.transform;
        return t == owner || t.IsChildOf(owner); // 자식 콜라이더도 자기 자신으로 간주
    }

    float DoHitAndComputeLength(Vector2 o, Vector2 d, float len, bool pierce)
    {
        const float skin = 0.05f;
        o += d * skin;

        if (pierce)
        {
            var hits = Physics2D.RaycastAll(o, d, len, hitMask);
            for (int i = 0; i < hits.Length; i++)
            {
                if (IsSelf(hits[i])) continue;
                ApplyIfDamageableAndImpact(hits[i]);
            }
            return len; // 관통은 길이 유지
        }
        else
        {
            // *** 변경점: RaycastAll로 쏘고, '첫 번째 비-자기'만 사용 ***
            var hits = Physics2D.RaycastAll(o, d, len, hitMask);
            float best = len;
            bool found = false;

            for (int i = 0; i < hits.Length; i++)
            {
                if (IsSelf(hits[i])) continue;           // 자기 자신 스킵
                ApplyIfDamageableAndImpact(hits[i]);     // 데미지 1회만 줄 거면 이 줄은 루프 밖으로 빼도 됨
                best = Mathf.Max(0.01f, hits[i].distance);
                found = true;
                break; // 첫 비-자기 히트까지만
            }

            return found ? best : len;
        }
}
    void ApplyIfDamageableAndImpact(RaycastHit2D h)
    {
        if (!h.collider) return;
        if (owner && h.collider.transform == owner) return;

        // 1) 데미지/상태 부여
        var dmg = h.collider.GetComponent<IDamageable>();
        if (dmg != null)
        {
            var ctx = new HitContext { attacker = owner, damage = damage, statusTags = status };
            dmg.ApplyHit(ctx);
        }

        // 2) 임팩트 VFX 스폰 (ImpactAutoDespawn이 알아서 소멸)
        SpawnImpact(h.point, h.normal);
    }

    void SpawnImpact(Vector2 pos, Vector2 normal)
    {
        if (impactPrefab == null || pool == null) return;

        // 충돌 법선 방향을 바라보도록 회전(스프라이트 'right'가 정면이라 가정)
        Quaternion rot = (normal.sqrMagnitude > 0.0001f)
            ? Quaternion.FromToRotation(Vector3.right, new Vector3(normal.x, normal.y, 0f))
            : Quaternion.identity;

        // 풀에서 가져와 스폰 → ImpactAutoDespawn 컴포넌트가 수명에 맞춰 PoolObject.Despawn() 호출
        pool.Get(impactPrefab, pos, rot, null);
    }

    void Despawn()
    {
        CancelInvoke();
        po?.Despawn();
    }
}