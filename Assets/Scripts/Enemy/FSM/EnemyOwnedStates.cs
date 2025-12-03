using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyOwnedStates
{
    public class SpawnState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            // 공통 스폰 로직
            enemy.ResetEnemy(); // 필수 초기화만
        }

        public void Execute(Enemy enemy)
        {
            enemy.ChangeState(EnemyStates.PatrolState);
        }

        public void FixedExecute(Enemy enemy)
        {
            
        }
        public void Exit(Enemy enemy) { }
    }

    public class PatrolState : IEnemyState
    {
        public void Enter(Enemy enemy) 
        { 
            enemy.ReAllocCurrentPos(); // 현재 위치 재설정
        }

        public void Execute(Enemy enemy)
        {
            if (enemy.IsInChaseRange())
                enemy.ChangeState(EnemyStates.ChaseState);
        }
        public void FixedExecute(Enemy enemy)
        {
            enemy.PatrolAround(); // 좌우 주변 순찰
        }
        public void Exit(Enemy enemy) { /* 애니메이션 정리 */ }
    }

    public class ChaseState : IEnemyState
    {
        public void Enter(Enemy enemy) { /* 애니메이션 시작 */ }

        public void Execute(Enemy enemy)
        {
            if (enemy.IsInAttackRange())
                enemy.ChangeState(EnemyStates.AttackState);
            
            if (!enemy.IsInChaseRange())
                enemy.ChangeState(EnemyStates.PatrolState);
        }
        public void FixedExecute(Enemy enemy)
        {
            enemy.MoveToTarget();
        }
        public void Exit(Enemy enemy) { /* 애니메이션 정리 */ }
    }

    public class AttackState : IEnemyState
    {
        bool isAttacking = false;
        public void Enter(Enemy enemy)
        {
            isAttacking = false;
            enemy.StartCoroutine(AttackCoroutine(enemy));
        }

        public void Execute(Enemy enemy)
        {
            if (isAttacking) return;

            /*
            if (!enemy.IsInAttackRange())
            {
                enemy.ChangeState(EnemyStates.ChaseState);
                return;
            }
            */

            //enemy.DoAttack();
            //enemy.StartCoroutine(AttackCoroutine(enemy));
        }
        private IEnumerator AttackCoroutine(Enemy enemy)
        {
            enemy.DoAttack();
            yield return new WaitForSeconds(1f);
            enemy.ChangeState(EnemyStates.ChaseState);

            /*
            isAttacking = true;

            enemy.Animator.SetTrigger("IsAttack");
            enemy.DoAttack(); // 몬스터별 공격 구현 호출

            AnimatorStateInfo stateInfo = enemy.Animator.GetCurrentAnimatorStateInfo(0); // 재생중인 애니메이션 길이 가져옴
            
            yield return new WaitForSeconds(stateInfo.length);

            isAttacking = false;
            */
        }
        public void FixedExecute(Enemy enemy)
        {

        }
        public void Exit(Enemy enemy) { }
    }

    public class DieState : IEnemyState
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
