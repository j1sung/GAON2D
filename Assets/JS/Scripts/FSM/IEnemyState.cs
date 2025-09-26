public interface IEnemyState
{
    void Enter(Enemy enemy);
    void Execute(Enemy enemy); // �Ϲ� ������(Update)
    void FixedExecute(Enemy enemy); // ���� ó����(FixedUpdate)
    void Exit(Enemy enemy);
}