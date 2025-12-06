using UnityEngine;

public class BlueprintPickup : BasePickup
{
    [SerializeField] private BlueprintSO blueprint;

    protected override bool TryPickup(PlayerAgents target)
    {
        if (target == null || blueprint == null) return false;
        return target.inventory.PickupBlueprint(blueprint);
    }
}