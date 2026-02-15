using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Effects/ReverseControlDamage")]
public class ReverseEffect : ShopEffectSO
{
    public override void Apply(PlayerContext ctx, ShopItemSO item)
    {
        if (PlayerController.Instance != null)
            PlayerController.Instance.SetControlReversed(true);

        ctx.Status.StatsModifier(StatKind.Damage, 10f);
    }
}