using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{   
    [SerializeField] private PlayerStatus status;

    [Header("Move")]
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    public Vector2 Movement => movement;

    [Header("Dash")]
    bool isDashing;
    float dashEndTime;
    float nextDashTime;
    // public event Action OnDash; 나중에 대쉬 UI 정해지면 추가

    [SerializeField] private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        // 대쉬
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= nextDashTime)
        {
            isDashing = true;
            dashEndTime = Time.time + status.RUN_dashDuration;
            nextDashTime = Time.time + status.RUN_dashCooldown;
        }
        if (isDashing && Time.time >= dashEndTime) isDashing = false;
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.MovePosition(rb.position + movement * status.RUN_dashSpeed * Time.fixedDeltaTime);
            // OnDash?.Invoke(); 
        }
        else
            rb.MovePosition(rb.position + movement * status.RUN_moveSpeed * Time.fixedDeltaTime);

        if (movement == Vector2.zero) // 멈출 때 잔여 속도 제거
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}