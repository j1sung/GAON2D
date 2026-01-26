using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBrainBase<TState> : MonoBehaviour, IEnemyBrain
    where TState : Enum
{
    protected Enemy enemy;
    protected EStateMachine.EnemyStateMachine fsm = new EStateMachine.EnemyStateMachine { debugLog = true };
    protected Dictionary<TState, IEnemyState<Enemy>> states;

    public void Init(Enemy enemy)
    {
        this.enemy = enemy;
        states = BuildStates(); // 각 브레인이 자기 상태만 만든다 (분기 없음)
    }

    public void OnEnableBrain()
    {
        if (states == null) states = BuildStates();
        fsm.ChangeState(GetEntryState(), states, enemy);
    }

    public void Tick()
    {
        if (!enemy.isLive) return;
        fsm.Update(enemy);
    }

    public void FixedTick()
    {
        if (!enemy.isLive) return;
        fsm.FixedUpdate(enemy);
    }

    // Brain이 자기 타입 ChangeState 제공
    public void ChangeState(TState s)
    {
        fsm.ChangeState(s, states, enemy);
    }

    protected abstract Dictionary<TState, IEnemyState<Enemy>> BuildStates();
    protected abstract TState GetEntryState();
}
