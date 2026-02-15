using UnityEngine;

public abstract class ShopEffectSO : ScriptableObject
{
    public abstract void Apply(PlayerContext ctx, ShopItemSO item);
}