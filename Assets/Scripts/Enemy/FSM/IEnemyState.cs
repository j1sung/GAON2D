public interface IEnemyState
{
    void Enter(Enemy enemy);
    void Execute(Enemy enemy); // 老馆 肺流侩(Update)
    void FixedExecute(Enemy enemy); // 拱府 贸府侩(FixedUpdate)
    void Exit(Enemy enemy);
}