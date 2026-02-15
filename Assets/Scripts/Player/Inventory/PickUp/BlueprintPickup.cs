using UnityEngine;

public class BlueprintPickup : BasePickup
{
    [SerializeField] private BlueprintSO blueprint;

    protected override bool TryPickup(PlayerContext target)
    {
        if (target == null || blueprint == null) return false;
        return target.Inventory.PickupBlueprint(blueprint);
    }
}