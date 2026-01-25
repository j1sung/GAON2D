public enum EnemyTier { Normal, MidBoss, Boss }

public interface IEnemy : IDamageable, IAttacker
{
    EnemyTier Tier { get; }
}
