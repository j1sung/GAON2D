using UnityEngine;

public class CorePickup : BasePickup
{
    [SerializeField] private CoreSO core;

    protected override bool TryPickup(PlayerAgents target)
    {
        if (target == null || core == null) return false;
        return target.inventory.PickupCore(core);
    }
}