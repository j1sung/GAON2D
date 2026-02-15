using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Effects/CritChanceUp")]
public class CritEffect : ShopEffectSO
{
    public override void Apply(PlayerContext ctx, ShopItemSO item)
    {
        ctx.Status.StatsModifier(StatKind.Critical, 0.1f);
    }
}