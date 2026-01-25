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

    private EnemyBase enemy;
    private Transform target;
    
    [SerializeField] private float moveSpeed = 3f;

    [Header("Ranges")]
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float attackRange = 2f;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        anim = GetComponentInChildren<Animator>();
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
        Debug.Log("[EnemyState] UpdateChase");
        if (anim != null)
        {
            bool moving = true;
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

        Vector2 dir = EnemyUtil.Direction2D(transform, target);
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
    }

    private void UpdateAttack()
    {
        if (target == null)
        {
            ChangeState(State.Idle);
            return;
        }

        float dist = EnemyUtil.Distance2D(transform, target);


        if (dist > attackRange)
        {
            ChangeState(State.Chase);
            return;
        }

        enemy.Attack();
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
