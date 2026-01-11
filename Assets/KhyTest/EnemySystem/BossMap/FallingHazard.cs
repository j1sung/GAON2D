using System;
using UnityEngine;

/*
 * 스폰될때 이미 타켓 위치 시작
 * N초 동안 애니메이션만 재생
 * N초 후 실제 피격 판정 시작
 * 재생 후 풀로 반납
 */

public class FallingHazard : MonoBehaviour
{
    [Header("Movement")]
    //[SerializeField] private float fallSpeed = 12f;
    //[SerializeField] private float xFollowSpeed = 20f;
    
    [Header("Life")]
    [SerializeField] private float activeDuration = 2.0f;
    [SerializeField] private Animator animator;
    [SerializeField] private string playStateName = "Play";


    [SerializeField] private float lifeTime = 5f;

    private Vector2 targetPos;
    private bool active;
    private float timer;

    private Action<FallingHazard> release;  

    public void Spawn(Vector2 targetPos, Action<FallingHazard> releaseCallback)
    {
        /*this.targetPos = targetPos;
        release = releaseCallback;

        timer = 0f;
        active = true;
        gameObject.SetActive(true);*/
        transform.position = targetPos;

        release = releaseCallback;
        timer = 0f;
        active = true;
        gameObject.SetActive(true);

        if (animator != null && !string.IsNullOrEmpty(playStateName))
            animator.Play(playStateName, 0, 0f);

    }

    private void Update()
    {
        if (!active) return;

        /*Vector2 pos = transform.position;

        pos.x = Mathf.Lerp(pos.x, targetPos.x, Time.deltaTime * xFollowSpeed);
        pos.y -= fallSpeed * Time.deltaTime;
        transform.position = pos;

        if (pos.y <= targetPos.y)
        {
            Despawn();
            return;
        }*/

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Despawn();
            //return;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var dmg = other.GetComponentInParent<IDamageable>();
        if (dmg != null)
        {
            dmg.ApplyHit(new HitContext
            {
                attacker = transform,
                damage = 10f,
                statusTags = default
            });
        }

        Despawn();
    }

    private void Despawn()
    {
        if (!active) return;
        active = false;

        if (release != null) release(this);
        else gameObject.SetActive(false);
    }
}
