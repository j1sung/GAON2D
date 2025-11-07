using EnemyOwnedStates;
using System.Collections;
using UnityEngine;

public enum EnemyStates { SpawnState = 0, ChaseState, AttackState, DieState }

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyStatsController status { get; private set; }
    public IEnemyAction action { get; private set; }

    private IEnemyState[] states; // Enemy가 가진 모든 상태 인스턴스 저장
    private EnemyFSM fsm = new EnemyFSM();

    [SerializeField] private EnemyData enemyData; // 적의 데이터를 가져옴
    [SerializeField] private DropTable dropTable; // 아이템 데이터 가져옴
    [SerializeField] private Transform dropParent; // 아이템 계층 부모

    public bool isLive { get; private set; } = false;

    Rigidbody2D target;
    Rigidbody2D rigid;
    Animator animator;

    public Animator Animator => animator;

    SpriteRenderer spriter;
    private Color originalColor;
    private MaterialPropertyBlock mpb;

    // === 중복 타격 방지(한 프레임에 레이저+트리거 동시 충돌 시 1회만 처리) ===
    private int _lastHitFrame = -9999;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        action = GetComponent<IEnemyAction>();
        status = new EnemyStatsController(enemyData);

        states = new IEnemyState[4];
        states[(int)EnemyStates.SpawnState] = new EnemyOwnedStates.SpawnState();
        states[(int)EnemyStates.ChaseState] = new EnemyOwnedStates.ChaseState();
        states[(int)EnemyStates.AttackState] = new EnemyOwnedStates.AttackState();
        states[(int)EnemyStates.DieState] = new EnemyOwnedStates.DieState();

        originalColor = spriter.color;
        mpb = new MaterialPropertyBlock();

        dropParent = GameObject.Find("====== Item ======").transform;
    }

    private void OnEnable()
    {
        ChangeState(EnemyStates.SpawnState);
    }

    private void Start()
    {
        // 최초 적 오브젝트 생성시 초기화
        target = GameInstance.Instance.player.GetComponent<Rigidbody2D>();
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

    private void LateUpdate()
    {
        if (!isLive || target == null) return;
        spriter.flipX = target.position.x < rigid.position.x; // 적 좌우 변환
    }

    // =========== SpawnState ===========

    public void ResetEnemy()
    {
        isLive = true;
        status.ResetStats();
    }

    // =========== ChaseState ===========
    public void MoveToTarget()
    {
        // 적 -> 플레이어 방향 = 위치차이 정규화
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * status.Speed * Time.fixedDeltaTime; // 프레임 독립 이동
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    // =========== AttackState ===========
    public bool IsInAttackRange()
    {
        // 나와 타겟 사이의 거리가 공격 사거리보다 짧거나 같으면 true
        return Vector2.Distance(transform.position, target.position) <= status.AttackRange;
    }

    public void DoAttack()
    {
        action?.Attack(transform, status.Damage);
    }

    // =========== DieState ===========
    public void Die()
    {
        isLive = false;
        ItemDrop(); // 죽을때 아이템 드랍
        gameObject.SetActive(false);
    }

    // 추후 ItemManager나 다른 클래스로 분리해서 구현
    public void ItemDrop()
    {
        float totalWeight = 0f;
        foreach (var entry in dropTable.entries)
            totalWeight += entry.weight;

        float rand = Random.Range(0, totalWeight);
        float sum = 0f;
        GameObject selected = null;

        foreach (var entry in dropTable.entries)
        {
            sum += entry.weight;
            if (rand <= sum)
            {
                selected = entry.prefab;
                break;
            }
        }

        if (selected != null)
        {
            GameObject itemObj = Instantiate(selected, GetRandomSpawnPos(), Quaternion.identity);
            itemObj.transform.SetParent(dropParent, true);
        }

        GameObject expObj = Instantiate(dropTable.exp, GetRandomSpawnPos(), Quaternion.identity);
        expObj.transform.SetParent(dropParent, true);
    }

    private Vector3 GetRandomSpawnPos() // 드랍 아이템 생성 위치 랜덤
    {
        Vector2 offset = Random.insideUnitCircle * 2f; // 반경 0.5유닛 안에서 랜덤 위치
        return transform.position + new Vector3(offset.x, offset.y, 0f);
    }

    // =========================
    //  레이저/총알 공통 데미지 진입점
    // =========================
    public void ApplyHit(HitContext ctx)
    {
        if (!isLive) return;

        // 같은 프레임 중복 타격(태그 트리거 + 레이저 Raycast) 방지
        if (_lastHitFrame == Time.frameCount) return;
        _lastHitFrame = Time.frameCount;

        status.TakeDamage(ctx.damage);

        // 상태이상 적용이 필요하면 여기서 ctx.statusTags를 참조해 처리
        // e.g., status.ApplyStatus(ctx.statusTags);

        // 피격 연출 원하면 주석 해제
        // StartCoroutine(HitFlash());

        if (status.IsDead)
            ChangeState(EnemyStates.DieState);
    }

    // ========== 레거시: 태그 기반 총알 트리거 ==========
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLive) return;

        // 총알/레이저 등 피해 콜라이더 태그
        if (!other.CompareTag("Damage")) return;

        // 같은 프레임에 이미 ApplyHit로 처리됐다면 스킵
        if (_lastHitFrame == Time.frameCount) return;

        // 1) 신형: 총알도 IDamageable 경로로 넣는 경우가 많아서,
        //    상대 스크립트가 우리에게 ApplyHit를 호출해줬다면 여기선 아무 것도 안해도 됨.

        // 2) 구형(호환): Attack.Pooling.Bullet 같이 public damage 필드를 직접 읽던 총알
        var pooledBullet = other.GetComponent<Attack.Pooling.Bullet>();
        if (pooledBullet != null)
        {
            _lastHitFrame = Time.frameCount;
            status.TakeDamage(pooledBullet.damage);
            // StartCoroutine(HitFlash()); //적 피격 VFX
            if (status.IsDead) ChangeState(EnemyStates.DieState);
            return;
        }

        // 3) (선택) 아주 옛날 네임스페이스: Attack.Bullet
        //    여기서는 damage가 private일 수 있어 직접 처리하지 않고,
        //    총알 측 OnTriggerEnter2D에서 IDamageable.ApplyHit()를 호출하도록 유지하는 편이 안전함.
        //    => 별도 처리 없음
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