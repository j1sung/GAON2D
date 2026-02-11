using UnityEngine;

public class BossEnemy : EnemyBase
{
    [Header("Boss Pattern")]
    [SerializeField] private BossPatternManager patternManager;

    [Header("Attack Control")]
    [SerializeField] private float attackCooldown = 2.0f;

    private float _nextAttackTime;

    private Animator anim;

    private static readonly int DieTrigger = Animator.StringToHash("die");
    private static readonly int IsMove = Animator.StringToHash("isMove");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");

    private void Reset()
    {
        tier = EnemyTier.Boss;
        patternManager = GetComponent<BossPatternManager>(); // 같은 오브젝트에 붙이는 기준.
    }

    protected override void Awake()
    {
        base.Awake();


        anim = GetComponentInChildren<Animator>();

        if (!enabled) return; // stats null이면 base에서 enabled=false 처리했을 수 있음.

        if (patternManager == null)
        {
            patternManager = GetComponent<BossPatternManager>();
            if (patternManager == null)
            {
                Debug.LogError($"{name}: BossPatternManager is missing.", this);
                enabled = false;
                return;
            }
        }
    }

    public override void Attack()
    {

        // 쿨타임 없으면 FSM UpdateAttack에서 매 프레임 공격해버림.
        if (Time.time < _nextAttackTime) return;
        _nextAttackTime = Time.time + attackCooldown;

        // 보스 전용 공격 로직
        patternManager.ExecuteRandomPattern(this, AttackPower);
    }

    protected override void Die()
    {
        // 보스 전용 사망 처리 (연출/드랍/컷신 등)
        // 예: patternManager.StopAll(); 같은 거 넣고 싶으면 여기서 처리.
        Debug.Log($"{name}: Boss died.");

        Debug.Log($"anim null? {anim == null}", this);
        Debug.Log($"controller null? {anim != null && anim.runtimeAnimatorController == null}", this);

        var state = GetComponent<EnemyState>();
        if (state) state.enabled = false;

        anim.SetBool(IsMove, false);
        anim.SetBool(IsAttack, false);

        anim.Play("Die", 0, 0f);

    }

    public void FinalizeDeath()
    {
        Destroy(gameObject);
    }
}
