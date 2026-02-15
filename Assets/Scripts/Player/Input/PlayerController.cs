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
    private bool _isControlReversed; // 좌우반전
    

    [Header("Dash")]
    bool isDashing;
    float dashEndTime;
    float nextDashTime;
    [SerializeField] private float afterImageInterval = 0.025f; // 잔상 간격
    private float nextAfterImageTime; // 다음 잔상 시간
    private bool _dashDisabled; // 대쉬 막기
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

        if (_isControlReversed)
        {
            moveX *= -1f;
            moveY *= -1f;
        }

        if (moveX > 0 && _inputEnabled) _sr.flipX = true;
        else if (moveX < 0 && _inputEnabled) _sr.flipX = false;


        if (isDashing) _anim.enabled = false;
        UpdateAnimator();

        // 대쉬
        if (!_dashDisabled && Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= nextDashTime)
        {
            StartDash();
        }

        if (isDashing && Time.time >= dashEndTime)
        {
            EndDash();
        }

        // 대쉬 중일때 nextAfterImageTime 간격으로 현재 프레임의 스프라이트를 찍는다.
        if (isDashing && Time.time >= nextAfterImageTime)
        {
            SpawnAfterImage();
            nextAfterImageTime = Time.time + afterImageInterval;
        }
        
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

    // ==================================================
    // Dash
    // ==================================================
    private void StartDash()
    {   
        _status.SetInvincible(true);
        isDashing = true;
        dashEndTime = Time.time + _status.RUN_dashDuration;
        nextDashTime = Time.time + _status.RUN_dashCooldown;

        // 전이 경쟁 무시: 즉시 Dash로 스냅
        _anim.ResetTrigger("DashTrigger");
        _anim.Play("Player_Dash", 0, 0f);
        _anim.Update(0f); // 바로 반영(선택)

        nextAfterImageTime = Time.time; 

        _anim.SetTrigger("DashTrigger");
    }

    private void EndDash()
    {
        isDashing = false;

        // 애니메이터를 켜면서 Walk 상태로 강제 지정
        _anim.enabled = true; 
        _anim.Play("Player_Walk", 0, 0f);
        _anim.Update(0f);
        
        _status.SetInvincible(false);
    }

    public void DisableDash()
    {
        _dashDisabled = true;
    }

    // 잔상효과 연출 - 일정 프레임 간격으로 이미지 복사
    void SpawnAfterImage()
    {
        var go = new GameObject("AfterImage_TMP");
        var sr = go.AddComponent<SpriteRenderer>();

        // 위치 / 정렬
        go.transform.position = transform.position;
        go.transform.localScale = transform.localScale;

        // 스프라이트 복사
        sr.sprite = _sr.sprite;
        sr.flipX = _sr.flipX;
        sr.sortingLayerID = _sr.sortingLayerID;
        sr.sortingOrder = _sr.sortingOrder - 1;

        // 알파값 조정
        var c = _sr.color;
        c.a = 0.6f;
        sr.color = c;

        // 0.15초 뒤 제거
        Destroy(go, 0.15f);
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

    private void MovementStop()
    {
        _inputEnabled = false;
    }

    // 입력 좌우반전
    public void SetControlReversed(bool value)
    {
        _isControlReversed = value;
    }

    private void UpdateAnimator()
    {
        if (!_anim) return;

        // Dash 중엔 Speed로 상태 흔들지 않음
        if (!isDashing)
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