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
            enemy.InitEnemy(); // 최초 적 오브젝트 생성시 호출
            enemy.ResetEnemy(); // 풀에서 꺼내면 매번 호출
        }

        public void Execute(Enemy enemy)
        {
            enemy.ChangeState(EnemyStates.ChaseState);
        }

        public void FixedExecute(Enemy enemy)
        {
            
        }
        public void Exit(Enemy enemy) { }
    }

    public class ChaseState : IEnemyState
    {
        public void Enter(Enemy enemy) { /* 애니메이션 시작 */ }

        public void Execute(Enemy enemy)
        {
            if (enemy.IsInAttackRange())
                enemy.ChangeState(EnemyStates.AttackState);
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
        }

        public void Execute(Enemy enemy)
        {
            if (isAttacking) return;

            // 공격은 비동기로 가야하나? 혹은 Invoke()?
            // 애니메이션 모션 끝날때를 트리거로 쿨타임 가지고 범위 내라면 계속 공격

            if (!enemy.IsInAttackRange())
            {
                enemy.ChangeState(EnemyStates.ChaseState);
                return;
            }

            //enemy.StartCoroutine(AttackCoroutine(enemy));
        }
        private IEnumerator AttackCoroutine(Enemy enemy)
        {
            isAttacking = true;

            enemy.Animator.SetTrigger("IsAttack");
            enemy.DoAttack(); // 몬스터별 공격 구현 호출

            AnimatorStateInfo stateInfo = enemy.Animator.GetCurrentAnimatorStateInfo(0); // 재생중인 애니메이션 길이 가져옴
            
            yield return new WaitForSeconds(stateInfo.length);

            isAttacking = false;
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
