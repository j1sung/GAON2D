using UnityEngine;

public class AutoCollector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {   
        var orb = other.GetComponent<AutoPickupable>();
        if (orb == null) return;
        orb.StartMoveTo(transform);
    }
}