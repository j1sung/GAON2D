using UnityEngine;

public class BlueprintPickup : BasePickup
{
    [SerializeField] private BlueprintSO blueprint;

    protected override bool TryPickup()
    {
        if (targetInv == null || blueprint == null) return false;
        return targetInv.PickupBlueprint(blueprint);
    }
}