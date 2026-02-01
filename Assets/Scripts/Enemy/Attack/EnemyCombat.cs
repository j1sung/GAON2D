using UnityEngine;

// 적 공격모드 선택
public enum AttackMode
{
    None = 0,
    Normal,
    Skill,
}

public sealed class EnemyCombat : MonoBehaviour
{
    [SerializeField] private HitBox hitbox; // 자식 Hitbox 콜라이더 연결

    private EnemyStatsController status;
    private EnemyMovement movement;
    private IEnemyAction action;
    private Animator animator;

    // 적 공격모드 선택
    private AttackMode _mode;

    // 적 공격 유형 세팅
    private void SetMode(AttackMode mode){ _mode = mode; }

    private void SetBodyDamage(AttackMode mode)
    {
        hitbox.BodyDamage(mode == AttackMode.Skill ? status.SkillDamage : status.NormalDamage);
    }

    public void Init(EnemyStatsController stats, EnemyMovement movement, IEnemyAction action, Animator animator)
    {
        this.status = stats;
        this.movement = movement;
        this.action = action;
        this.animator = animator;

        if (!hitbox)
        {
            Debug.LogError($"[{name}] EnemyCombat hitbox is null. (프리팹 연결 or 자식 HitBox 필요)");
            hitbox = GetComponentInChildren<HitBox>(true);
        }
        if (status == null)
        {
            Debug.LogError($"[{name}] EnemyCombat status is null.");
            return;
        }

        SetMode(AttackMode.Normal); // 기본 공격 유형 설정 -> 일반 공격
        SetBodyDamage(AttackMode.Normal); // 기본 접촉 데미지 설정 -> 일반 데미지
    }

    // 일반 공격
    public void DoAttack()
    {
        // 애니메이션 처리
        animator.SetTrigger("Attack");

        // 공격 유형 변경 -> 일반 공격.
        SetMode(AttackMode.Normal);

        // 공격 동작 실행
        AttackValue value = new AttackValue(status.NormalDamage, movement.Target.position, _mode);
        action?.Attack(in value);
    }

    // 스킬 공격
    public void DoSkill()
    {
        // 애니메이션 처리
        animator.SetTrigger("Skill");

        // 공격 유형 변경 -> 스킬 공격.
        SetMode(AttackMode.Skill);

        // 접촉 데미지 적용 -> 스킬 데미지
        SetBodyDamage(AttackMode.Skill); 

        // 공격 동작 실행
        AttackValue value = new AttackValue(status.SkillDamage, movement.Target.position, _mode);
        action?.Attack(in value);
    }

    // ==== 애니 클립 이벤트에서 호출 ====

    // 일반 공격 끝나면
    public void OnAttackAnimFinished()
    {
        // “0 데미지로 끄기” 같은 트릭은 이제 필요 없어지는 게 보통이라
        // action 쪽에서 종료 처리가 필요하면 별도 메서드로 분리하는 게 더 깔끔함.
        // action?.EndAttack(); 같은 식으로

        AttackValue value = default; // 기본값(0 초기화) 넣기
        action?.Attack(in value); // 데미지 끄기 용 임시
    }

    // 스킬 끝나면
    public void OnSkillAnimFinished()
    {
        SetMode(AttackMode.Normal);    // 다시 일반 데미지로 복귀
    }
}
