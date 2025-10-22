using UnityEngine;

public abstract class BasePickup : MonoBehaviour
{
    protected bool inRange;
    protected Inventory inventory;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        inventory = other.GetComponent<Inventory>();
        inRange = inventory != null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (other.GetComponent<Inventory>() == inventory)
        {
            inRange = false;
            inventory = null;
        }
    }

    protected abstract bool TryPickup();

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            if (TryPickup())
                Destroy(gameObject); // 성공시 제거
        }
    }
}