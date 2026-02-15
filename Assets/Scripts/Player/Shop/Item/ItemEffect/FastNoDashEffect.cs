using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Effects/SpeedNoDash")]
public class FastNoDashEffect : ShopEffectSO
{
    public override void Apply(PlayerContext ctx, ShopItemSO item)
    {
        ctx.Status.StatsModifier(StatKind.MoveSpeed, 5f);

        if (PlayerController.Instance != null)
            PlayerController.Instance.DisableDash();
    }
}