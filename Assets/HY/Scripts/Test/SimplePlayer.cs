using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimplePlayer: MonoBehaviour
{
    private static SimplePlayer instance;

    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 move;
    void Awake() { 
        rb = GetComponent<Rigidbody2D>(); 
        
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void Update()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }
}
