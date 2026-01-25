using System.Collections.Generic;

public class NormalBrain : EnemyBrainBase<NormalState>
{
    protected override NormalState GetEntryState() => NormalState.SpawnState;

    protected override Dictionary<NormalState, IEnemyState<Enemy>> BuildStates()
    {
        var map = new Dictionary<NormalState, IEnemyState<Enemy>>(8)
        {
            [NormalState.SpawnState] = new EnemyStateSpace.Normal.SpawnState_Normal(),
            [NormalState.PatrolState] = new EnemyStateSpace.Normal.PatrolState_Normal(),
            [NormalState.ChaseState] = new EnemyStateSpace.Normal.ChaseState_Normal(),
            [NormalState.AttackState] = new EnemyStateSpace.Normal.AttackState_Normal(),
            [NormalState.DieState] = new EnemyStateSpace.Normal.DieState_Normal(),
        };
        return map;
    }
}
