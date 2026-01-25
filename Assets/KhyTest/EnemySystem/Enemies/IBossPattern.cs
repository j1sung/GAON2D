using UnityEngine;

public interface IBossPattern
{
    void Execute(EnemyBase boss, Transform target, float attackPower);
}
