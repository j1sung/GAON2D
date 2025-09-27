using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;

    private Animator anim;

    private Vector3 movement;
    public Vector3 Movement => movement;

    // ▼ 대쉬 변수
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    bool isDashing;
    float dashEndTime;
    float nextDashTime;
    Vector3 dashDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // ▼ 추가
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        movement = new Vector3(moveX, 0f, moveZ).normalized;

        if (anim != null)
        {
            anim.SetFloat("Speed", movement.sqrMagnitude);
            anim.SetBool("isDashing", isDashing);
        }

        // 좌우 반전
        if (moveX != 0) spriteRenderer.flipX = (moveX > 0);

        // --- 대쉬
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

        if (movement == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}