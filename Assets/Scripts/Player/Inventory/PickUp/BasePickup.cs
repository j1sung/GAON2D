using UnityEngine;

public abstract class BasePickup : MonoBehaviour
{
    [SerializeField] protected bool autoCollect = false;
    [SerializeField] protected KeyCode interactKey = KeyCode.E;

    protected bool inRange;
    protected PlayerContext receiver;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var agents = other.GetComponentInParent<PlayerAgents>();
        receiver = agents != null ? agents.Context : null;
        inRange = receiver != null;

        // 아이템 자동 줍기
        if (autoCollect && inRange)
        {
            if (TryPickup(receiver)) Despawn();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var agents = other.GetComponentInParent<PlayerAgents>();
        if (agents != null && receiver == agents.Context)
        {
            inRange = false;
            receiver = null;
        }
    }

    private void Update()
    {
        if (!autoCollect && inRange && Input.GetKeyDown(interactKey))
        {
            if (TryPickup(receiver)) Despawn();
        }
    }

    protected abstract bool TryPickup(PlayerContext target);

    protected virtual void Despawn()
    {
        Destroy(gameObject);
    }
}