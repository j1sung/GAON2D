using UnityEngine;

public interface IEnemyAction
{
    void Attack(in AttackValue attackValue);


}

public readonly struct AttackValue
{
    public readonly float damage;
    public readonly Vector2 targetPos;

    public AttackValue(float damage, Vector2 targetPos)
    {
        this.damage = damage;
        this.targetPos = targetPos;
    }
}