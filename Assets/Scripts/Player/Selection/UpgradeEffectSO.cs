using UnityEngine;

public abstract class UpgradeEffectSO : ScriptableObject
{
    public abstract void ApplyEffect(PlayerContext ctx);
}