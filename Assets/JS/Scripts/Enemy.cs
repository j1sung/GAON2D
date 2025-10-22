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

    private IEnemyState[] states; // Enemy가 가진 모든 상태 인스턴스 저장
    private EnemyFSM fsm = new EnemyFSM();

    public EnemyData enemyData; // 적의 데이터를 가져옴
    [SerializeField]private DropTable dropTable; // 아이템 데이터 가져옴
    [SerializeField] private Transform dropParent; // 아이템 계층 부모

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
        if (!isLive) return;
        spriter.flipX = target.position.x < rigid.position.x; // 적 좌우 변환
    }

    // =========== SpawnState ===========
    public void InitEnemy()
    {
        if (isInitialized) // 풀에 존재한다면
        {
            return;
        }
        // 최초 적 오브젝트 생성시 초기화

        target = GameInstance.Instance.player.GetComponent<Rigidbody2D>();

        //animator.runtimeAnimatorController = enemyData.controller; // 상태에 따른 애니메이션 전환인데 추후 정상화
        status.InitStatus(enemyData);

        isInitialized = true;
    }

    public void ResetEnemy()
    {
        isLive = true;
        status.ResetStatus();
    }

    // =========== ChaseState ===========
    public void MoveToTarget()
    {
        // 적 -> 플레이어 방향 = 위치차이 정규화
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * status.Speed * Time.fixedDeltaTime; // 다음 벡터: fixedDeltaTime -> 프레임 영향을 받지 않게
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    // =========== AttackState ===========
    public bool IsInAttackRange()
    {
        // 나와 타겟 사이의 거리가 공격 사거리보다 짧거나 같으면 true, 아니면 false 반환
        return Vector2.Distance(transform.position, target.position) <= status.AttackRange; // 적마다 공격 사거리가 다를 수 있음
    }
    
    public void DoAttack()
    {
        action?.Attack(this);
    }

    // =========== DieState ===========
    public void Die()
    {
        isLive = false;
        ItemDrop(); // 죽을때 아이템 드랍
        gameObject.SetActive(false);
    }

    // 추후 ItemManager나 다른 클래스로 분리해서 구현
    /* 
    어떤 아이템을 스폰할지? -> 스크립터블 데이터로 1.설계도, 2.코어(메인, 서브)가 있음 => 프리펩에 데이터 넣으면 해당 아이템이 될 수 있음
    그럼 이 3개중에서 뭐를 스폰할지는 확률적으로 정하는 로직이 필요함
    일단 1개 item 스폰 -> 빈 프리펩을 준비하고 확률적으로 정해진 아이템의 스크립터블 데이터를 넣어서 스폰하면 됨
    아이템 드랍 위치 설정 추가
    */
    public void ItemDrop()
    {
        float totalWeight = 0f;
        foreach(var entry in dropTable.entries)
            totalWeight += entry.weight;

        float rand = Random.Range(0, totalWeight);
        float sum = 0f;
        GameObject selected = null; // ItemData selected = null;

        foreach(var entry in dropTable.entries)
        {
            sum += entry.weight;
            if(rand <= sum)
            {
                selected = entry.prefab; // entry.item;
                break;
            }
        }

        if (selected != null) 
        {
            // 빈 프리펩 Instantiate
            GameObject itemObj = Instantiate(selected, transform.position, Quaternion.identity);

            // DropItemData 세팅
            itemObj.transform.SetParent(dropParent, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 총알 태그를 Damage로 설정
        if (other.CompareTag("Damage"))
        {
            // 총알이 가진 Bullet스크립트에서 damage 가져오기
            Attack.Pooling.Bullet bullet = other.GetComponent<Attack.Pooling.Bullet>();
            status.ReduceHealth(bullet.damage);

            // 피격 시 색상 반짝이기
            // StartCoroutine(HitFlash());

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

    // 공격 범위 기즈모
    void OnDrawGizmos()
    {
        if (status == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status.AttackRange);
    }

    // 임시 체력 UI

    /*
    private string label = "";

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
}
