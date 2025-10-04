using UnityEngine;

public abstract class BasePickup : MonoBehaviour
{
    protected bool inRange;
    protected Inventory targetInv;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        targetInv = other.GetComponent<Inventory>();
        inRange = targetInv != null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (other.GetComponent<Inventory>() == targetInv)
        {
            inRange = false;
            targetInv = null;
        }
    }

    protected abstract bool TryPickup(); // 자식이 실제 픽업 로직 구현

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            if (TryPickup())
                Destroy(gameObject); // 성공시 제거
        }
    }
}