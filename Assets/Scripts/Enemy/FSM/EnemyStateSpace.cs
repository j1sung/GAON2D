using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace EnemyStateSpace.Normal
{
    // ==== Normal Enemy States ====
    public class SpawnState_Normal : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            // 공통 스폰 로직
           // enemy.ResetEnemy(); // 필수 초기화만
            // SpawnState가 isLive를 true로 안 켜면 영원히 Tick이 안 돈다 → 이거만 주의.
        }

        public void Execute(Enemy enemy)
        {
            enemy.ChangeState(NormalState.PatrolState);
        }

        public void FixedExecute(Enemy enemy)
        {
            
        }
        public void Exit(Enemy enemy) { }
    }

    public class PatrolState_Normal : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy) 
        { 
            enemy.ReAllocCurrentPos(); // 현재 위치 재설정
        }

        public void Execute(Enemy enemy)
        {
            if (enemy.IsInChaseRange())
                enemy.ChangeState(NormalState.ChaseState);
        }
        public void FixedExecute(Enemy enemy)
        {
            enemy.PatrolAround(); // 좌우 주변 순찰
        }
        public void Exit(Enemy enemy) { /* 애니메이션 정리 */ }
    }

    public class ChaseState_Normal : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy) { /* 애니메이션 시작 */ }

        public void Execute(Enemy enemy)
        {
            if (enemy.IsInAttackRange()) // 공격 범위 감지 -> 공격 상태 전환
                enemy.ChangeState(NormalState.AttackState);
            
            if (!enemy.IsInChaseRange()) // 플레이어 범위 멀어짐 -> 탐색 상태 전환
                enemy.ChangeState(NormalState.PatrolState);
        }
        public void FixedExecute(Enemy enemy)
        {
            enemy.MoveToTarget();
        }
        public void Exit(Enemy enemy) { /* 애니메이션 정리 */ }
    }

    public class AttackState_Normal : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            enemy.StartStateRoutine(AttackCoroutine(enemy));
        }

        public void Execute(Enemy enemy) { }
        private IEnumerator AttackCoroutine(Enemy enemy)
        {
            enemy.DoAttack();

            // 여기서 공격 후에 임시로 잠깐 쉬고 다음 상태로 넘어가게 함.
            // 여기서 쉰다고 애니메이션 전환이 멈추는건 아님 -> Attack 애니메이션을 play하고 바로 기존 애니메이션으로 돌아가기 때문.
            yield return new WaitForSeconds(3.5f);

            enemy.ChangeState(NormalState.ChaseState);
        }
        public void FixedExecute(Enemy enemy) { }
        public void Exit(Enemy enemy) { }
    }

    public class DieState_Normal : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            enemy.Die();
        }

        public void Execute(Enemy enemy) { }
        public void FixedExecute(Enemy enemy)
        {

        }
        public void Exit(Enemy enemy) { }
    }
}

namespace EnemyStateSpace.MidBoss
{
    // ==== MidBoss Enemy States ====

    public class SpawnState_MidBoss : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            // SpawnState가 isLive를 true로 안 켜면 영원히 Tick이 안 돈다 → 이거만 주의.
            throw new System.NotImplementedException();
        }

        public void Execute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Exit(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void FixedExecute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ChaseState_MidBoss : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Execute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Exit(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void FixedExecute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AttackState_MidBoss : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Execute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Exit(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void FixedExecute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }
    }

    public class DieState_MidBoss : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Execute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Exit(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void FixedExecute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }
    }
}

namespace EnemyStateSpace.Boss
{
    // ==== Boss Enemy States ====
    public class SpawnState_Boss : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            // SpawnState가 isLive를 true로 안 켜면 영원히 Tick이 안 돈다 → 이거만 주의.
            throw new System.NotImplementedException();
        }

        public void Execute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Exit(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void FixedExecute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ChaseState_Boss : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Execute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Exit(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void FixedExecute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }
    }
    public class AttackState_Boss : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Execute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Exit(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void FixedExecute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }
    }
    public class DieState_Boss : IEnemyState<Enemy>
    {
        public void Enter(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Execute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void Exit(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public void FixedExecute(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }
    }
}