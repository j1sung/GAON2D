using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimplePlayer: MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 move;
    void Awake() { rb = GetComponent<Rigidbody2D>(); }
    void Update()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }
}
