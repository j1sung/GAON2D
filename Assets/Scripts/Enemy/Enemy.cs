using EnemyOwnedStates;
using System.Collections;
using UnityEngine;

public enum EnemyStates { SpawnState = 0, PatrolState, ChaseState, AttackState, DieState }

public class Enemy : MonoBehaviour, IEnemy, IDamageable
{
    [SerializeField] private EnemyData enemyData; // 적의 데이터를 가져옴
    [SerializeField] private DropTable dropTable; // 아이템 데이터 가져옴
    [SerializeField] private Transform dropParent; // 아이템 계층 부모

    public EnemyType Type => enemyData.Type;
    public EnemyStatsController status { get; private set; }
    public IEnemyAction action { get; private set; }
    private IEnemyBrain brain;

    private IEnemyState[] states; // Enemy가 가진 모든 상태 인스턴스 저장
    private EnemyFSM fsm = new EnemyFSM { debugLog = true };

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

    // 기능 구현 모듈
    private EnemyMovement movement;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();

        action = GetComponent<IEnemyAction>();
        //brain = GetComponent<IEnemyBrain>();
        status = new EnemyStatsController(enemyData);

        states = new IEnemyState[5];
        states[(int)EnemyStates.SpawnState] = new EnemyOwnedStates.SpawnState();
        states[(int)EnemyStates.PatrolState] = new EnemyOwnedStates.PatrolState();
        states[(int)EnemyStates.ChaseState] = new EnemyOwnedStates.ChaseState();
        states[(int)EnemyStates.AttackState] = new EnemyOwnedStates.AttackState();
        states[(int)EnemyStates.DieState] = new EnemyOwnedStates.DieState();

        originalColor = spriter.color;
        mpb = new MaterialPropertyBlock();

        dropParent = GameObject.Find("====== Item ======").transform;

        // 모듈 초기화
        movement = GetComponent<EnemyMovement>();
        if (movement == null)
            movement = gameObject.AddComponent<EnemyMovement>();
        movement.Init(status, rigid, spriter);
    }

    private void OnEnable()
    {
        ChangeState(EnemyStates.SpawnState);
    }

    private void Start()
    {
        // 최초 적 오브젝트 생성시 초기화
        target = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        movement.SetTarget(target);
    }

    public void ChangeState(EnemyStates newstate)
    {
        fsm.ChangeState(newstate, states, this);
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

    public void DoAttack()
    {
        // 애니메이션 재생 임시 여기서 재생
        animator.SetTrigger("Attack");

        // 매개변수는 구조체로 공통으로 만들어 선택적으로 넘기게 만들어야 할듯
        action?.Attack(status.Damage, lastPlayerPos = movement.Target.position); // 임시로 마지막 플레이어 위치 넣음
    }

    // 애니 이벤트로 호출 -> 적 데미지 비활성화
    public void OnAttackAnimFinished()
    {
        action?.Attack(0f, Vector2.zero); // 데미지 끄기 용 임시
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
            ChangeState(EnemyStates.DieState);
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