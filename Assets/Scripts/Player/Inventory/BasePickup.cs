using UnityEngine;

public abstract class BasePickup : MonoBehaviour {
    [SerializeField] protected bool autoCollect = false;        // 경험치 등 자동 습득
    [SerializeField] protected KeyCode interactKey = KeyCode.E; // 수동 습득 키

    protected bool inRange;
    protected PlayerAgents receiver;

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        receiver = other.GetComponent<PlayerAgents>();
        inRange = receiver != null;

        // 자동 습득이면 들어오자마자 시도
        if (autoCollect && inRange) {
            if (TryPickup(receiver)) Despawn();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        if (other.GetComponent<PlayerAgents>() == receiver) {
            inRange = false;
            receiver = null;
        }
    }

    // 필요 시 OnTriggerStay2D에서 자동흡수 거리 보정/자력 흡입 등 확장 가능

    private void Update() {
        if (!autoCollect && inRange && Input.GetKeyDown(interactKey)) {
            if (TryPickup(receiver)) Despawn();
        }
    }

    protected abstract bool TryPickup(PlayerAgents target);

    // 나중에 아이템 Pool로 관리하게 되면 수정
    protected virtual void Despawn() {
        Destroy(gameObject); 
    }
}