using UnityEngine;

public interface IEnemyAction
{
    void Attack(float Damage, Vector2 targetPos);
}