using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM
{
    public IEnemyState currentState { get; private set; }

    public bool debugLog;

    public void ChangeState(EnemyStates newstate, IEnemyState[] states, Enemy enemy)
    {
        var prev = currentState;
        
        // 상태 관련 코루틴/Invoke 정리
        enemy.StopStateRoutine();
        enemy.CancelInvoke(); // Invoke는 선택

        // 현재 재생중인 상태가 있으면 Exit() 메소드 호출
        currentState?.Exit(enemy);

        // 새로운 상태로 변경 후, 새로 바뀐 상태의 Enter() 메소드 호출
        currentState = states[(int)newstate];

        if (debugLog)
        {
            string prevName = prev?.GetType().Name ?? "None";
            string nextName = currentState?.GetType().Name ?? "None";
            Debug.Log($"[FSM] {enemy.name}: {prevName} -> {nextName}");
        }

        currentState.Enter(enemy);
    }

    public void Update(Enemy enemy) => currentState?.Execute(enemy);
    public void FixedUpdate(Enemy enemy) => currentState?.FixedExecute(enemy);

}
