using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    public float damage = 50f;

    private float timer = 0f;
    private Rigidbody rb;
 

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            //sr.color = Color.white;
            gameObject.SetActive(false); // 파괴 대신 비활성화
        }
    }

    public void Fire(Vector3 direction)
    {
        direction.y = 0f;
        direction.Normalize();
        rb.velocity = direction * speed;


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;

        if (other.CompareTag("Enemy"))
        {
            // Enemy 태그 객체의 콜라이더를 감지하면 적의 체력을 담당하는 클래스를 가져옴. 
            /* EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        } */

            gameObject.SetActive(false);
        }
    }

    /*
    void OnDisable()
    {
        SpriteRenderer sr;
        sr = transform.Find("bulletSprite")?.GetComponentInChildren<SpriteRenderer>();
        if(sr != null)
            sr.color = Color.white;
    }
    */
}