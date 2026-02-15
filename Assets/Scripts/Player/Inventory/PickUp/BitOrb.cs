using UnityEngine;

public class BitOrb : AutoPickupable
{
    [SerializeField] private int _bitAmount = 1;

    protected override void OnCollected(Transform player)
    {
        if (!player) return; 

        var currency = player.GetComponentInParent<PlayerCurrency>();
        if (currency) currency.AddBits(_bitAmount);
    }
}