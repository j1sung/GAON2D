using UnityEngine;
using Attack.Pooling;

public sealed class WeaponManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform owner;
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private Inventory inventory;
    [SerializeField] private CameraRecoil cameraRecoil;

    [Header("Fire Points")]
    [SerializeField] public Transform firePoint; // 기본무기용
    [SerializeField] public Transform combinedFirePoint; // 조합무기용

    [Header("Default Weapon")]
    [SerializeField] private WeaponSO defaultWeapon;

    // 기본 무기
    private WeaponController _defaultCtl;

    // 선택된 조합 무기
    private WeaponController _combinedCtl;

    // PlayerAim.cs에서 갱신
    public Vector2 aimDir_Default;
    public Vector2 aimDir_Combined;

    // 슬롯 교체 쿨타임
    private float _switchCooldown = 3f;
    private float _switchTimer = 0f;


    void Awake()
    {
        if (!inventory) inventory = FindObjectOfType<Inventory>();

        var rt = new RuntimeWeapon(defaultWeapon);
        _defaultCtl = new WeaponController(rt);
        _defaultCtl.Setup(new FireInitContext {
            runtime = rt,
            owner   = owner,
            pool    = poolManager,
            subCore = null
        });
        _defaultCtl.OnFire  += cameraRecoil.Fire;
    }
    
    void OnEnable()
    {
        if (inventory != null) inventory.OnWeaponsChanged += HandleWeaponsChanged;
    }

    void OnDisable()
    {
        if (inventory != null) inventory.OnWeaponsChanged -= HandleWeaponsChanged;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        _switchTimer -= dt;

        // 각 무기의 쿨타임 계속 감소
        _defaultCtl?.TickCooldown(dt);
        _combinedCtl?.TickCooldown(dt);

        // 우클릭, StartBurst에서 초기화하면서 Burst가 시작된다.
        _combinedCtl?.SingleFire(combinedFirePoint, aimDir_Combined, owner, dt);
    }

    // 기본 공격
    public void FireDefault(float dt)
    {
        _defaultCtl?.ContinousFire(firePoint, aimDir_Default, owner, dt);
    }

    // 조합 무기 공격
    public void FireCombined()
    {
        _combinedCtl?.StartBurst();
    }

    // 인벤토리에서 "선택된 조합 무기"가 바뀔 때마다 재구성
    private void HandleWeaponsChanged()
    {
        var selected = inventory.GetSelectedCombinedWeapon();


        // 기존 조합 무기 정리
        if (_combinedCtl != null)
        {
            _combinedCtl.OnFire -= cameraRecoil.Fire;
            _combinedCtl = null;
        }

        if (selected == null) return;

        var rt = new RuntimeWeapon(selected);
        var ctl = new WeaponController(rt);

        // SubCore 생성
        var sub = MapCoreToSubModule(selected.coreA) ?? MapCoreToSubModule(selected.coreB);

        // Setup
        ctl.Setup(new FireInitContext {
            runtime = rt,
            owner   = owner,
            pool    = poolManager,
            subCore = sub
        });

        // 등록
        _combinedCtl = ctl;
        _combinedCtl.OnFire += cameraRecoil.Fire;
    }

    // ====== 자동 공격 ======
    // void Update()
    // {
    //     if (!firePoint || !owner) return;
    //     float dt = Time.deltaTime;

    //     // 기본 무기 항상 발사 루프
    //     _defaultCtl?.TickAndFire(firePoint, _aimDir, owner, dt);

    //     // 선택된 조합 무기 병행 발사(있을 때만)
    //     _combinedCtl?.TickAndFire(combinedFirePoint, _aimDir, owner, dt);

    //     // 쿨타임 타이머 감소
    //     if (_switchTimer > 0f)
    //         _switchTimer -= dt;
    // }


    // 슬롯 교체 코드
    public void Select(int index)
    {
        if (_switchTimer > 0f)
        {
            Debug.Log($"[WeaponManager] 전환 대기중... {Mathf.CeilToInt(_switchTimer)}초 남음");
            return;
        }

        _switchTimer = _switchCooldown;
        inventory.SelectCombinedWeapon(index); // 인벤토리로 실제 전환 요청
        Debug.Log($"[WeaponManager] 슬롯 {index + 1} 전환 완료 (쿨타임 {_switchCooldown}초)");
    }

    // --- 외부 입력 ---
    public void SetDefaultAimDir(Vector3 mouseWorldPos)
    {
        Vector3 dir3 = mouseWorldPos - firePoint.position;
        Vector2 dir = new Vector2(dir3.x, dir3.y);  // Z 제거
        if (dir.sqrMagnitude > 0.0001f)
            aimDir_Default = dir.normalized;
    }

    public void SetCombinedAimDir(Vector3 mouseWorldPos)
    {
        Vector3 dir3 = mouseWorldPos - combinedFirePoint.position;
        Vector2 dir = new Vector2(dir3.x, dir3.y);
        if (dir.sqrMagnitude > 0.0001f)
            aimDir_Combined = dir.normalized;
    }

    // 공격중인지 여부
    public bool IsCombinedFiring 
    {
        get { return _combinedCtl != null && _combinedCtl.IsBurstFiring; }
    }

    //CoreSO → SubCore 매핑
    private ISubCoreModule MapCoreToSubModule(CoreSO core)
    {
        if (core == null || core.type != CoreSO.CoreType.sub) return null;

        string s = ((core.code ?? "") + " " + (core.coreName ?? "")).ToUpperInvariant();

        if (s.Contains("ACCEL"))
            return new AccelCore();     // 가속

        if (s.Contains("MULTI"))
            return new SpreadCore();    // 확산

        return null;
    }
}