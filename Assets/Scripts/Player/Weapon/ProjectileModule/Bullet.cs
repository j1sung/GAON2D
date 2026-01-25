using UnityEngine;
using Attack.Pooling; // PoolManager

namespace Attack.Pooling
{
    public sealed class Bullet : MonoBehaviour
    {
        // ---- 런타임 파라미터 ----
        private Transform owner;
        private Vector2 dir;
        private float speed;
        private float life;
        public float damage;
        private StatusTag status;

        // ---- 컴포넌트 ----
        private Rigidbody2D rb;

        [Header("Impact VFX (pooled or fallback)")]
        [SerializeField] private GameObject hitEffectPrefab;     // ImpactVFX 프리팹(권장: PoolObject 포함)
        [SerializeField] private float fallbackImpactLife = 0.25f; // 풀 미사용 시 파괴 대기 시간

        // 중복 처리 방지(충돌/만료 동시 발생 대비)
        private bool dead;

        public struct Args
        {
            public Transform owner;
            public Vector2 dir;
            public float speed;
            public float life;
            public float damage;
            public StatusTag status;
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void OnEnable()
        {
            dead = false;
            CancelInvoke();
        }

        public void Init(in Args a)
        {
            owner  = a.owner;
            dir    = (a.dir == Vector2.zero) ? Vector2.right : a.dir.normalized;
            speed  = a.speed;
            life   = a.life;
            damage = a.damage;
            status = a.status;

            // 각도 보정
            transform.up = dir;

            if (rb) rb.velocity = dir * speed;

            CancelInvoke();
            Invoke(nameof(Expire), life); // 수명 만료 시에도 이펙트 생성
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (dead) return;

            // 콜라이더 레이어 체크
            if (other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                return;

            IDamageable dmg = other.GetComponentInParent<IDamageable>();

            if (dmg != null)
            {
                var hit = new HitContext
                {
                    attacker   = owner,
                    damage     = damage,
                    statusTags = status
                };
                dmg.ApplyHit(hit);
            }

            SpawnHitEffect();
            Despawn();
        }

        // 수명 만료 경로
        void Expire()
        {
            if (dead) return;
            SpawnHitEffect();
            Despawn();
        }

        private void SpawnHitEffect()
        {
            if (!hitEffectPrefab) return;

            // 풀 우선, 없으면 Instantiate 폴백
            if (PoolManager.instance != null)
            {
                PoolManager.instance.Get(hitEffectPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                var go = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                // 폴백: 자동 반환 스크립트가 없으면 일정 시간 후 파괴
                if (!go.TryGetComponent<ImpactAutoDespawn>(out _))
                {
                    Destroy(go, fallbackImpactLife);
                }
            }
        }

        public void Despawn()
        {
            if (dead) return;
            dead = true;

            CancelInvoke();
            if (rb) rb.velocity = Vector2.zero;

            // PoolObject가 붙어 있으면 Despawn() → PoolManager.Release로 자동 연결
            if (TryGetComponent<PoolObject>(out var po))
            {
                po.Despawn();
            }
            else
            {
                // 풀 관리 대상이 아니면 안전하게 비활성화
                gameObject.SetActive(false);
            }
        }
    }
}