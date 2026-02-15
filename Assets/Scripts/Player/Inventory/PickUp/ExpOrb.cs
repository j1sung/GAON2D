using UnityEngine;

public class ExpOrb : AutoPickupable
{
    [SerializeField] private int expAmount = 10;

    protected override void OnCollected(Transform player)
    {
        if (!player) return; 

        var status = player.GetComponentInParent<PlayerStatus>();
        if (status) status.GainExp(expAmount);
    }
}