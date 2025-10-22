using UnityEngine;

public class BlueprintPickup : BasePickup
{
    [SerializeField] private BlueprintSO blueprint;

    protected override bool TryPickup()
    {
        if (inventory == null || blueprint == null) return false;
        return inventory.PickupBlueprint(blueprint);
    }
}