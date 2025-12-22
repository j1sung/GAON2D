using UnityEngine;

public class CorePickup : BasePickup
{
    [SerializeField] private CoreSO core;

    protected override bool TryPickup(PlayerContext target)
    {
        if (target == null || core == null) return false;
        return target.Inventory.PickupCore(core);
    }
}