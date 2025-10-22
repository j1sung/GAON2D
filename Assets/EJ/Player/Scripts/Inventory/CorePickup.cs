using UnityEngine;

public class CorePickup : BasePickup
{
    [SerializeField] private CoreSO core;

    protected override bool TryPickup()
    {
        if (inventory == null || core == null) return false;
        return inventory.PickupCore(core);
    }
}