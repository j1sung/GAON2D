using UnityEngine;

public interface IDamageable
{
    void ApplyHit(HitContext ctx);
}

public struct HitContext
{
    public Transform attacker;
    public float damage;
    public StatusTag statusTags;
}