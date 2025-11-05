using UnityEngine;

public class ExpCollector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {   
        var orb = other.GetComponent<ExpOrb>();
        if (orb != null)
        {
            orb.StartMoveTo(transform);
        }
    }
}