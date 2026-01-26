using EnemyStateSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, EIEnemy.IEnemy, IDamageable
{
    [SerializeField] private EnemyData enemyData; // 적의 데이터를 가져옴
    [SerializeField] private DropTable dropTable; // 아이템 데이터 가져옴
    [SerializeField] private Transform dropParent; // 아이템 계층 부모

    public EnemyType Type => enemyData.Type;
    public EnemyStatsController status { get; private set; }
    public IEnemyAction action { get; private set; }

    // ==== FSM brain으로 나중에 빠질 예정 ====
    private IEnemyBrain brain; // 추후 brain 식으로 변경

    // ======================================

    public bool isLive { get; private set; } = false;

    private Vector2 lastPlayerPos;

    private Rigidbody2D target;
    private Rigidbody2D rigid;

    private Animator animator;

    private SpriteRenderer spriter;

    private Color originalColor;
    private MaterialPropertyBlock mpb;

    // === 중복 타격 방지(한 프레임에 레이저+트리거 동시 충돌 시 1회만 처리) ===
    private int _lastHitFrame = -9999;

    private Coroutine _stateRoutine;

    // ==== 기능 구현 모듈 ====
    
    // 추적/패트롤/범위 체크/이동
    [SerializeField]private EnemyMovement movement;

    // 공격을 언제 할지 + 히트박스 on/off + 공격 쿨/중복히트/타겟 스냅샷
    [SerializeField] private EnemyCombat combat; //-> 전투(공격) 기능 분리

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();

        action = GetComponent<IEnemyAction>();
        status = new EnemyStatsController(enemyData);

        // ==== brain 세팅 ====
        brain = GetComponent<IEnemyBrain>();
        if (brain == null)
            Debug.LogError($"[{name}] IEnemyBrain 컴포넌트가 없음. NormalBrain/MidBossBrain/BossBrain 중 하나 붙여야 함.");
        else
            brain.Init(this);

        originalColor = spriter.color;
        mpb = new MaterialPropertyBlock();

        dropParent = GameObject.Find("====== Item ======").transform;

        // 모듈 초기화
        movement = GetComponent<EnemyMovement>();
        if (movement == null)
            movement = gameObject.AddComponent<EnemyMovement>();
        movement.Init(status, rigid, spriter);

        combat = GetComponent<EnemyCombat>();
        if(combat == null)
            combat = gameObject.AddComponent<EnemyCombat>();
        combat.Init(status, movement, action, animator);
    }

    private void OnEnable()
    {
        if (brain == null)
        {
            brain = GetComponent<IEnemyBrain>();
            if (brain != null) brain.Init(this);
        }
        brain?.OnEnableBrain();
    }

    private void Start()
    {
        // 최초 적 오브젝트 생성시 초기화
        target = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        movement.SetTarget(target);
    }

    public void ChangeState<TState>(TState s) where TState : System.Enum
    {
        if (brain is EnemyBrainBase<TState> typed)
            typed.ChangeState(s);
        else
            UnityEngine.Debug.LogError($"[{name}] Brain 타입과 ChangeState enum 타입이 안 맞음: {typeof(TState).Name}");
    }


    private void Update()
    {
        brain?.Tick();
    }

    void FixedUpdate()
    {
        brain?.FixedTick();
    }

    public void StartStateRoutine(IEnumerator routine)
    {
        StopStateRoutine();
        _stateRoutine = StartCoroutine(routine);
    }

    public void StopStateRoutine()
    {
        if(_stateRoutine != null)
        {
            StopCoroutine(_stateRoutine);
            _stateRoutine = null;
        }
    }

    // =========== SpawnState ===========

    public void ResetEnemy()
    {
        isLive = true;
        status.ResetStats();
        animator.ResetTrigger("Die");
    }

    // =========== PatrolState ===========
    //private bool goingLeft = true;
    //private float leftX;
    //private float rightX;

    public void ReAllocCurrentPos() // 위치 재설정
    {
        movement.ReAllocCurrentPos(transform.position);
    }
    public void PatrolAround() // 주변 순찰
    {
        movement.PatrolAround();
    }

    //private float chaseRange = 7f;
    public bool IsInChaseRange() // Chase 범위 체크
    {
        return movement.IsInChaseRange();
    }

    // =========== ChaseState ===========

    public void MoveToTarget()
    {
        movement.MoveToTarget();
    }
    public bool IsInAttackRange()
    {
        return movement.IsInAttackRange(status.AttackRange);
    }

    // =========== AttackState ===========

    public void DoAttack() // -> 적 데미지/콜라이더 활성화
    {
        combat.DoAttack();
    }

    // 애니클립 이벤트로 호출 -> 적 데미지/콜라이더 비활성화
    public void OnAttackAnimFinished()
    {
        combat.OnAttackAnimFinished();
    }

    // =========== DieState ===========
    public void Die()
    {
        isLive = false;
        ItemDrop(); // 죽을때 아이템 드랍
        SectorController.Instance.OnEnemyDied(); // 죽은 갯수 카운트

        animator.SetTrigger("Die");
        //gameObject.SetActive(false);
    }

    // 애니 이벤트로 호출 -> 적 비활성화
    public void OnDeathAnimFinished()
    {
        gameObject.SetActive(false);
    }

    // 추후 ItemManager나 다른 클래스로 분리해서 구현
    public void ItemDrop()
    {
        /*
        // 랜덤한 값 뽑기
        float totalWeight = 0f;
        foreach (var entry in dropTable.entries)
            totalWeight += entry.weight;

        float rand = Random.Range(0, totalWeight);
        float sum = 0f;
        GameObject selected = null;

        // 랜덤 값에 걸리는 아이템 선택
        foreach (var entry in dropTable.entries)
        {
            sum += entry.weight;
            if (rand <= sum)
            {
                selected = entry.prefab;
                break;
            }
        }

        // 선택된 아이템 드랍
        if (selected != null)
        {
            GameObject itemObj = Instantiate(selected, GetRandomSpawnPos(), Quaternion.identity);
            itemObj.transform.SetParent(dropParent, true);
        }
        */

        GameObject expObj = Instantiate(dropTable.exp, transform.position, Quaternion.identity);
        expObj.transform.SetParent(dropParent, true);
    }

    private Vector3 GetRandomSpawnPos() // 드랍 아이템 생성 위치 랜덤
    {
        Vector2 offset = Random.insideUnitCircle * 2f; // 반경 0.5유닛 안에서 랜덤 위치
        return transform.position + new Vector3(offset.x, offset.y, 0f);
    }

    // =========================
    //  적이 공격 받음
    // =========================
    public void ApplyHit(HitContext ctx)
    {
        if (!isLive) return;

        // 같은 프레임 중복 타격(태그 트리거 + 레이저 Raycast) 방지
        if (_lastHitFrame == Time.frameCount) return;
        _lastHitFrame = Time.frameCount;

        status.TakeDamage(ctx.damage);

        if (status.IsDead)
            ChangeState(NormalState.DieState);
    }

    // 적 피격 VFX
    private IEnumerator HitFlash()
    {
        spriter.material.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        spriter.material.color = originalColor;
    }

    // 공격 범위 기즈모
    void OnDrawGizmos()
    {
        if (status == null) return;

        // Chase 범위
        Gizmos.color = Color.blue;
        float chaseRange = (movement != null) ? movement.ChaseRange : 7f;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // Attack 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status.AttackRange);
    }

    // (임시 체력 GUI는 주석 유지)
    /*
    private string label = "";
    private void OnGUI()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);

        int currentHP = Mathf.RoundToInt(status.CurrentHealth);
        int maxHP = Mathf.RoundToInt(status.MaxHealth);
        int cs = (fsm.currentState != null) ? (int)fsm.currentState.GetType().Name.GetHashCode() : 0;

        label = $"{fsm.currentState?.GetType().Name} | HP: {currentHP}/{maxHP}";
        GUI.Label(new Rect(screenPos.x - 100, Screen.height - screenPos.y - 140, 150, 20), label);
    }
    */
}