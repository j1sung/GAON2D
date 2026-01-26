using UnityEngine;

public sealed class EnemyCombat : MonoBehaviour
{
    [SerializeField] private HitBox hitbox; // 자식 Hitbox 콜라이더 연결

    private EnemyStatsController status;
    private EnemyMovement movement;
    private IEnemyAction action;
    private Animator animator;

    public void Init(EnemyStatsController stats, EnemyMovement movement, IEnemyAction action, Animator animator)
    {
        this.status = stats;
        this.movement = movement;
        this.action = action;
        this.animator = animator;

        hitbox?.Disarm(); // 콜라이더 비활성화/데미지 0 초기화
    }

    public void DoAttack()
    {
        animator.SetTrigger("Attack");

        // 타격 프레임이 시작되는 타이밍에만 켜고 싶으면
        // 여기서 Arm 하지 말고 애니 이벤트에서 Arm 호출해도 됨.

        // 공격 데미지 적용(콜라이더 처리)
        hitbox?.Arm(status.Damage);

        // 공격 스킬 실행
        AttackValue value = new AttackValue(status.Damage, movement.Target.position);
        action?.Attack(in value);
    }

    // 애니 이벤트에서 호출
    public void OnAttackAnimFinished()
    {
        // 콜라이더 비활성화, 데미지 0 초기화
        hitbox?.Disarm();

        // “0 데미지로 끄기” 같은 트릭은 이제 필요 없어지는 게 보통이라
        // action 쪽에서 종료 처리가 필요하면 별도 메서드로 분리하는 게 더 깔끔함.
        // action?.EndAttack(); 같은 식으로
        AttackValue value = new AttackValue(0f, Vector2.zero);
        action?.Attack(in value); // 데미지 끄기 용 임시
    }
}
