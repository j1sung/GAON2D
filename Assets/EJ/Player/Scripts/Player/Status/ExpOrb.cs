using Unity.VisualScripting;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int expAmount = 10;
    private Transform target;
    private bool isMoving = false;
    private bool isCollected = false;
    private float moveSpeed = 8f;

    void Update()
    {
        if (isMoving && !isCollected)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, target.position) < 0.3f)
            {
                isCollected = true;
                target.GetComponentInParent<PlayerStatus>()?.GainExp(expAmount);
                Destroy(gameObject);
            }
        }
    }

    public void StartMoveTo(Transform player)
    {
        target = player;
        isMoving = true;
    }
}