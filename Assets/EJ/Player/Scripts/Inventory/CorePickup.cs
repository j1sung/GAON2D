using UnityEngine;

public class CorePickup : BasePickup
{
    [SerializeField] private CoreSO core;

    protected override bool TryPickup()
    {
        if (targetInv == null || core == null) return false;
        return targetInv.PickupCore(core);
    }
}