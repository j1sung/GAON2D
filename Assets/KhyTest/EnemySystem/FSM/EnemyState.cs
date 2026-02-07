using UnityEngine;

[RequireComponent(typeof(EnemyState))]
public class EnemyState : MonoBehaviour
{
    private enum State
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }
    private State currentState;

    private Animator anim;
    private static readonly int IsMove = Animator.StringToHash("isMove");

    private static readonly int IsAttack = Animator.StringToHash("isAttack");

    private EnemyBase enemy;
    private Transform target;


    private SpriteRenderer spriteRenderer; 

    [SerializeField] private float moveSpeed = 3f;

    [Header("Ranges")]
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float attackRange = 2f;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Debug.Log($"Animator found? {anim != null}", this);
    }

    private void Start()
    {
        ChangeState(State.Idle);
    }

    private void Update()
    {
        Debug.Log($"[EnemyState] Update - State: {currentState}");

        switch (currentState)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Patrol:
                UpdatePatrol();
                break;
            case State.Chase:
                UpdateChase();
                break;
            case State.Attack:
                UpdateAttack();
                break;
        }
    }

    private void UpdateFacing()
    {
       if (spriteRenderer == null || target == null) 
       {
            return;
       }

        float dx = target.position.x - transform.position.x;

        if (Mathf.Abs(dx) < 0.01f) return;

        spriteRenderer.flipX = dx > 0f;
    }

    private void ChangeState(State next)
    {
        currentState = next;

    }

    private void UpdateIdle()
    {
        Debug.Log("[EnemyState] UpdateIdle");

        FindTarget();
        if (target != null)
        {

            ChangeState(State.Chase);
        }
    }

    private void UpdatePatrol()
    {
        Debug.Log("[EnemyState] UpdatePatrol");

        FindTarget();
        if (target != null)
            ChangeState(State.Chase);
    }

    private void UpdateChase()
    {
        bool moving = false;
        if (anim != null)
        {
            moving = true;
            anim.SetBool(IsMove, moving);
        }

        if (target == null)
        {
            ChangeState(State.Idle);
            return;
        }

        float dist = EnemyUtil.Distance2D(transform, target);

        if (dist <= attackRange)
        {
            ChangeState(State.Attack);
            return;
        }

        UpdateFacing();

        Vector2 dir = EnemyUtil.Direction2D(transform, target);
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
    }

    private void UpdateAttack()
    {
        Debug.Log("[EnemyState] UpdateAttack");

        bool attacking = false;

        if (anim != null)
        {
            attacking = true;
            anim.SetBool(IsAttack, attacking);
        }

        if (target == null)
        {
            ChangeState(State.Idle);
            return;
        }

        float dist = EnemyUtil.Distance2D(transform, target);

        if (dist > attackRange)
        {
            attacking = false;
            anim.SetBool(IsAttack, attacking);
            ChangeState(State.Chase);
            return;
        }

        //enemy.Attack();
    }

    public void AttackHit()
    {

        float dist = EnemyUtil.Distance2D(transform, target);
        if (dist > attackRange) return;

        var dmg = target.GetComponentInParent<IDamageable> ();
        if (dmg != null)
        {
            dmg.ApplyHit(new HitContext
            {
                damage = enemy.AttackPower,
            });
        }

        // Todo : 이펙트/사운드
    }

    public void AttackEnd()
    {
        anim.SetBool(IsAttack, false);
        ChangeState (State.Chase);

    }

    private void FindTarget()
    {
        if (target == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("[EnemyState] Target assigned");
            }
        }
    }

    private void MoveToTarget()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * Time.deltaTime * 2f;
    }

}
