using UnityEngine;

public class PlayerController : MonoBehaviour
{       
    public static PlayerController Instance { get; private set; }

    private PlayerStatus status;
    private WeaponManager weaponManager;

    [Header("Move")]
    private Rigidbody2D rb;
    private Vector2 movement;
    public Vector2 Movement => movement;
    private Animator anim;
    

    [Header("Dash")]
    bool isDashing;
    float dashEndTime;
    float nextDashTime;
    // public event Action OnDash; 나중에 대쉬 UI 정해지면 추가

    public InventoryPanel invPanel;

    void Start()
    {   
        status = GetComponent<PlayerStatus>();
        weaponManager = GetComponent<WeaponManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized;

        UpdateAnimator();

        // 대쉬
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= nextDashTime)
        {
            isDashing = true;
            dashEndTime = Time.time + status.RUN_dashDuration;
            nextDashTime = Time.time + status.RUN_dashCooldown;
        }
        if (isDashing && Time.time >= dashEndTime) isDashing = false;
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryPanel.Instance?.OpenInventory();
        }
        HandleWeaponInput();
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

    private void UpdateAnimator()
    {
        if (!anim) return;

        anim.SetFloat("Speed", movement.sqrMagnitude);
        anim.SetBool("isDashing", isDashing);
    }

    private void HandleWeaponInput()
    {
        float dt = Time.deltaTime;

        // 기본 무기: 왼쪽 클릭 (Held)
        if (Input.GetMouseButton(0))
            weaponManager.FireDefault(dt);

        // 조합 무기: 오른쪽 클릭 (단발)
        if (Input.GetMouseButtonDown(1))
            weaponManager.FireCombined();
    }


}