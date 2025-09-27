using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    [Header("Move")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Vector2 movement;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    bool isDashing;
    float dashEndTime;
    float nextDashTime;
    Vector2 dashDir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        // 애니메이션
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized;

        if (anim != null)
        {
            anim.SetFloat("Speed", movement.sqrMagnitude);
            anim.SetBool("isDashing", isDashing);
        }

        // 좌우 반전
        if (moveX != 0) spriteRenderer.flipX = (moveX > 0);

        // 대쉬
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= nextDashTime)
        {
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            nextDashTime = Time.time + dashCooldown;
        }
        if (isDashing && Time.time >= dashEndTime) isDashing = false;
    }

    void FixedUpdate()
    {
        if (isDashing)
            rb.MovePosition(rb.position + movement * dashSpeed * Time.fixedDeltaTime);
        else
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}