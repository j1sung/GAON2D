using System.Collections.Generic;

public class BossBrain : EnemyBrainBase<BossState>
{
    protected override BossState GetEntryState() => BossState.SpawnState;

    protected override Dictionary<BossState, IEnemyState<Enemy>> BuildStates()
    {
        var map = new Dictionary<BossState, IEnemyState<Enemy>>(8)
        {
            [BossState.SpawnState] = new EnemyStateSpace.Boss.SpawnState_Boss(),
            // Boss는 Patrol이 없으면 아예 enum에 없게 만들었을 거고, 있어도 여기서 안 넣으면 됨.
            [BossState.ChaseState] = new EnemyStateSpace.Boss.ChaseState_Boss(),
            [BossState.AttackState] = new EnemyStateSpace.Boss.AttackState_Boss(),
            [BossState.DieState] = new EnemyStateSpace.Boss.DieState_Boss(),
        };
        return map;
    }
}
