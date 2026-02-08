using UnityEngine;

public interface IEnemyAction
{
    void Attack(in AttackValue attackValue);


}

public readonly struct AttackValue
{
    public readonly float damage;
    public readonly Vector2 targetPos;
    public readonly AttackMode mode;

    public AttackValue(float damage, Vector2 targetPos, AttackMode mode)
    {
        this.damage = damage;
        this.targetPos = targetPos;
        this.mode = mode;
    }
}