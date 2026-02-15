using UnityEngine;

public abstract class AutoPickupable : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float collectDistance = 0.3f;

    private Transform target;
    private bool isMoving;
    private bool isCollected;

    void Update()
    {
        if (!isMoving || isCollected || target == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.position) <= collectDistance)
        {
            isCollected = true;
            OnCollected(target);   // 여기서 각자 습득 처리
            Destroy(gameObject);
        }
    }

    public void StartMoveTo(Transform player)
    {
        target = player;
        isMoving = true;
    }

    //  상속받는 쪽에서 습득하는 로직 구현
    protected abstract void OnCollected(Transform player);
}