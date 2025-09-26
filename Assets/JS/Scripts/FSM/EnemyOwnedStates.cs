using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyOwnedStates
{
    public class SpawnState : IEnemyState
    {
        public void Enter(Enemy enemy)
        {
            // ���� ���� ����
            enemy.InitEnemy(); // ���� �� ������Ʈ ������ ȣ��
            enemy.ResetEnemy(); // Ǯ���� ������ �Ź� ȣ��
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
        public void Enter(Enemy enemy) { /* �ִϸ��̼� ���� */ }

        public void Execute(Enemy enemy)
        {
            if (enemy.IsInAttackRange())
                enemy.ChangeState(EnemyStates.AttackState);
        }
        public void FixedExecute(Enemy enemy)
        {
            enemy.MoveToTarget();
        }
        public void Exit(Enemy enemy) { /* �ִϸ��̼� ���� */ }
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

            // ������ �񵿱�� �����ϳ�? Ȥ�� Invoke()?
            // �ִϸ��̼� ��� �������� Ʈ���ŷ� ��Ÿ�� ������ ���� ����� ��� ����

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
            enemy.DoAttack(); // ���ͺ� ���� ���� ȣ��

            AnimatorStateInfo stateInfo = enemy.Animator.GetCurrentAnimatorStateInfo(0); // ������� �ִϸ��̼� ���� ������
            
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
