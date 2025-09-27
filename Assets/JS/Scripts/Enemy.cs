using EnemyOwnedStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditorInternal.VersionControl.ListControl;

public enum EnemyStates { SpawnState = 0, ChaseState, AttackState, DieState }

public class Enemy : MonoBehaviour
{
    public EnemyStatus status { get; private set; }
    public IEnemyAction action { get; private set; }

    private IEnemyState[] states; // Enemy�� ���� ��� ���� �ν��Ͻ� ����
    private EnemyFSM fsm = new EnemyFSM();

    public EnemyData enemyData; // ���� �����͸� ������
    public bool isLive { get; private set; } = false;
    bool isInitialized = false;

    Rigidbody2D target;
    Rigidbody2D rigid;
    Animator animator;

    public Animator Animator => animator;
    
    SpriteRenderer spriter;
    private Color originalColor;
    private MaterialPropertyBlock mpb;

    //NavMeshAgent agent;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        action = GetComponent<IEnemyAction>();
        status = GetComponent<EnemyStatus>();

        //agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;
        //agent.updateUpAxis = false;

        states = new IEnemyState[4];
        states[(int)EnemyStates.SpawnState] = new EnemyOwnedStates.SpawnState();
        states[(int)EnemyStates.ChaseState] = new EnemyOwnedStates.ChaseState();
        states[(int)EnemyStates.AttackState] = new EnemyOwnedStates.AttackState();
        states[(int)EnemyStates.DieState] = new EnemyOwnedStates.DieState();

        originalColor = spriter.color;
        mpb = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        ChangeState(EnemyStates.SpawnState);
    }

    //private string label = "";

    /*
    private void OnGUI()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);

        int currentHP = Mathf.RoundToInt(status.CurrentHealth);
        int maxHP = Mathf.RoundToInt(status.MaxHealth);
        string currentState = fsm.currentState.GetType().Name;
        
        label = $"{currentState} | HP: {currentHP}/{maxHP}";

        GUI.Label(new Rect(screenPos.x - 100, Screen.height - screenPos.y - 140, 150, 20), label);
    }
    */

    public void InitEnemy()
    {
        if (isInitialized) return;

        target = GameInstance.Instance.player.GetComponent<Rigidbody2D>();

        //animator.runtimeAnimatorController = enemyData.controller;
        status.InitStatus(enemyData);

        isInitialized = true;
    }

    public void ResetEnemy()
    {
        isLive = true;
        status.ResetStatus();
    }

    public void ChangeState(EnemyStates newstate)
    {
        fsm.ChangeState(newstate, states, this);
    }

    public void MoveToTarget()
    {
        //agent.SetDestination(target.position);

        // �� -> �÷��̾� ���� = ��ġ���� ����ȭ
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * status.Speed * Time.fixedDeltaTime; // ���� ����: fixedDeltaTime -> ������ ������ ���� �ʰ�
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    public bool IsInAttackRange()
    {
        return Vector2.Distance(transform.position, target.position) <= status.AttackRange; // ������ ���� ��Ÿ��� �ٸ� �� ����
    }

    // �ӽ� ü�� UI
    void OnDrawGizmos()
    {
        if (status == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status.AttackRange);
    }

    public void DoAttack()
    {
        action?.Attack(this);
    }

    public void Die()
    {
        isLive = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isLive) return;
        fsm.Update(this);
    }

    void FixedUpdate()
    {
        if (!isLive) return;
        fsm.FixedUpdate(this);
    }

    private void LateUpdate()
    {
        if (!isLive) return;
        spriter.flipX = target.position.x < rigid.position.x; // �� �¿� ��ȯ
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damage"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            status.ReduceHealth(bullet.damage);

            // �ǰ� �� ���� ��¦�̱�
            StartCoroutine(HitFlash());

            if (status.IsDead)
            {
                ChangeState(EnemyStates.DieState);
            }
        }
    }

    private IEnumerator HitFlash()
    {
        spriter.material.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        spriter.material.color = originalColor;
    }
}
