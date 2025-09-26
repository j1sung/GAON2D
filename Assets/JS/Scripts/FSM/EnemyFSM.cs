using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM
{
    public IEnemyState currentState { get; private set; }

    public void ChangeState(EnemyStates newstate, IEnemyState[] states, Enemy enemy)
    {
        // ���� ������� ���°� ������ Exit() �޼ҵ� ȣ��
        currentState?.Exit(enemy);

        // ���ο� ���·� ���� ��, ���� �ٲ� ������ Enter() �޼ҵ� ȣ��
        currentState = states[(int)newstate];
        currentState.Enter(enemy);
    }

    public void Update(Enemy enemy) => currentState?.Execute(enemy);
    public void FixedUpdate(Enemy enemy) => currentState?.FixedExecute(enemy);

}
