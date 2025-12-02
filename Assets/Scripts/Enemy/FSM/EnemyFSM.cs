using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM
{
    public IEnemyState currentState { get; private set; }

    public void ChangeState(EnemyStates newstate, IEnemyState[] states, Enemy enemy)
    {
        // 현재 재생중인 상태가 있으면 Exit() 메소드 호출
        currentState?.Exit(enemy);

        // 새로운 상태로 변경 후, 새로 바뀐 상태의 Enter() 메소드 호출
        currentState = states[(int)newstate];
        currentState.Enter(enemy);
    }

    public void Update(Enemy enemy) => currentState?.Execute(enemy);
    public void FixedUpdate(Enemy enemy) => currentState?.FixedExecute(enemy);

}
