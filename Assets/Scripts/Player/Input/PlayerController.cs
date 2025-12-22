using UnityEngine;

public class PlayerController : MonoBehaviour
{       
    public static PlayerController Instance { get; private set; }

    private PlayerStatus _status;
    private WeaponManager _weaponManager;

    [Header("Move")]
    private Rigidbody2D _rb;
    public Vector2 movement { get; private set; }
    private Animator _anim;
    private SpriteRenderer _sr;
    public bool isDead; // 생사여부 판별 bool 변수
    private bool _inputEnabled; // Input 제어용 bool 변수
    

    [Header("Dash")]
    bool isDashing;
    float dashEndTime;
    float nextDashTime;
    // public event Action OnDash; 나중에 대쉬 UI 정해지면 추가

    public InventoryPanel invPanel;

    void Awake()
    {   
        _status = GetComponent<PlayerStatus>();
        _weaponManager = GetComponent<WeaponManager>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _inputEnabled = true;
        isDead = false;
    }

    void OnEnable()
    {
        _status.OnDeath += PlayerDie;
    }

    void OnDisable()
    {
        _status.OnDeath -= PlayerDie;
    }


    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized;

        if (moveX > 0 && _inputEnabled) _sr.flipX = true;
        else if (moveX < 0 && _inputEnabled) _sr.flipX = false;

        UpdateAnimator();

        // 대쉬
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= nextDashTime)
        {
            isDashing = true;
            dashEndTime = Time.time + _status.RUN_dashDuration;
            nextDashTime = Time.time + _status.RUN_dashCooldown;
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
            if(!_inputEnabled) return;
            _rb.MovePosition(_rb.position + movement * _status.RUN_dashSpeed * Time.fixedDeltaTime);
            // OnDash?.Invoke(); 나중에 대쉬 게이지가 추가되면 추가예정.
        }
        else // _inputEnabled가 false면 움직임 x
        {
            if(!_inputEnabled) return;
            _rb.MovePosition(_rb.position + movement * _status.RUN_moveSpeed * Time.fixedDeltaTime);
        }

        if (movement == Vector2.zero) // 멈출 때 잔여 속도 제거
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;
        }
    }

    private void MovementStop()
    {
        _inputEnabled = false;
    }

    private void PlayerDie()
    {   
        isDead = true;
        _weaponManager.enabled = false;
        MovementStop();

        // 물리 반응 stop
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void UpdateAnimator()
    {
        if (!_anim) return;

        _anim.SetFloat("Speed", movement.sqrMagnitude);
        _anim.SetBool("isDashing", isDashing);
        _anim.SetBool("isDead", isDead);
    }

    private void HandleWeaponInput()
    {
        float dt = Time.deltaTime;

        // 기본 무기: 왼쪽 클릭 (Held)
        if (Input.GetMouseButton(0))
            _weaponManager.FireDefault(dt);

        // 조합 무기: 오른쪽 클릭 (단발)
        if (Input.GetMouseButtonDown(1))
            _weaponManager.FireCombined();
    }


}