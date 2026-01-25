public interface IEnemyState<T>
{
    void Enter(T enemy);
    void Execute(T enemy); // 老馆 肺流侩(Update)
    void FixedExecute(T enemy); // 拱府 贸府侩(FixedUpdate)
    void Exit(T enemy);
}