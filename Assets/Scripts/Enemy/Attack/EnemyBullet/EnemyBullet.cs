using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;

    private EnemyBulletPooling ownerPool; // 소유권 주입

    private Rigidbody2D rb;
    private Vector2 nextVec;
    private float _damage;

    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = 0;
    }

    public void SetOwnerPool(EnemyBulletPooling pool) => ownerPool = pool;

public void Fire(Vector2 dir, float speed, float damage)
    {
        nextVec = dir * speed;
        _damage = damage;
        timer = 0;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + nextVec * Time.fixedDeltaTime);

        timer += Time.fixedDeltaTime;

        // 정해둔 생명 시간 끝나면 비활성.
        if(timer>lifeTime)
        {
            // 비활성화 해놓고 추후 풀로 자동 반환됨.
            gameObject.SetActive(false); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_damage <= 0f)
        {
            Debug.Log("damage: " + _damage);
            return;
        }

        // 콜라이더 레이어 체크
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) 
        {
            // 풀링 반환
            ownerPool.Release(gameObject);

            // 이펙트 풀링
            var v = ownerPool.GetVFX();
            v.transform.position = transform.position;

            return; 
        }

        IDamageable dmg = other.GetComponentInParent<IDamageable>();

        if (dmg == null)
        {
            Debug.Log("IDamageable 인식 못함!");
            return;
        }

        dmg.ApplyHit(new HitContext { damage =  _damage });

        // 풀링 반환
        ownerPool.Release(gameObject);

        // 이펙트 풀링
        var vfx = ownerPool.GetVFX();
        vfx.transform.position = transform.position;
    }
}
