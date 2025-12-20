using UnityEngine;

[CreateAssetMenu(menuName = "SO/UpgradeEffect/Status")]
public class UpgradeStatusSO : UpgradeEffectSO
{   
    public StatKind kind; // 증가시킬 스테이터스
    public float value; // 값

    public override void ApplyEffect(PlayerContext ctx)
    {
        ctx.Status.StatsModifier(kind, value);
    }
}
