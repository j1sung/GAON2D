using System.Collections.Generic;

public class MidBossBrain : EnemyBrainBase<MidBossState>
{
    protected override MidBossState GetEntryState() => MidBossState.SpawnState;

    protected override Dictionary<MidBossState, IEnemyState<Enemy>> BuildStates()
    {
        var map = new Dictionary<MidBossState, IEnemyState<Enemy>>(8)
        {
            [MidBossState.SpawnState] = new EnemyStateSpace.MidBoss.SpawnState_MidBoss(),
            [MidBossState.ChaseState] = new EnemyStateSpace.MidBoss.ChaseState_MidBoss(),
            [MidBossState.AttackState] = new EnemyStateSpace.MidBoss.AttackState_MidBoss(),
            [MidBossState.DieState] = new EnemyStateSpace.MidBoss.DieState_MidBoss(),
        };
        return map;
    }
}
