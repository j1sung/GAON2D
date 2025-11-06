using UnityEngine;
using Attack.Pooling;

public sealed class WeaponManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform owner;
    [SerializeField] private Attack.Pooling.PoolManager poolManager;
    [SerializeField] private Inventory inventory;

    [Header("Fire Points")]
    [SerializeField] private Transform firePoint; // 기본무기용
    [SerializeField] private Transform combinedFirePoint; // 조합무기용

    [Header("Default Weapon")]
    [SerializeField] private WeaponSO defaultWeapon;

    // 기본 무기
    private RuntimeWeapon _defaultRt;
    private WeaponController _defaultCtl;

    // 선택된 조합 무기
    private RuntimeWeapon _combinedRt;
    private WeaponController _combinedCtl;

    // PlayerAim.cs에서 갱신
    private Vector2 _aimDir = Vector2.right;

    // 슬롯 교체 쿨타임
    private float _switchCooldown = 3f;
    private float _switchTimer = 0f;

    void Awake()
    {
        if (!inventory) inventory = FindObjectOfType<Inventory>();

        _defaultRt  = new RuntimeWeapon(defaultWeapon);
        _defaultCtl = new WeaponController(_defaultRt);
        _defaultCtl.Setup(new FireInitContext {
            runtime = _defaultRt,
            owner   = owner,
            pool    = poolManager,
            subCore = null
        });
    }

    void OnEnable()
    {
        if (inventory != null) inventory.OnWeaponsChanged += HandleWeaponsChanged;
    }

    void OnDisable()
    {
        if (inventory != null) inventory.OnWeaponsChanged -= HandleWeaponsChanged;
    }

    // 인벤토리에서 "선택된 조합 무기"가 바뀔 때마다 재구성
    private void HandleWeaponsChanged()
    {
        var selected = inventory.GetSelectedCombinedWeapon();

        // 기존 조합 컨트롤러 해제
        _combinedRt  = null;
        _combinedCtl = null;

        if (selected == null) return;

        _combinedRt  = new RuntimeWeapon(selected);
        _combinedCtl = new WeaponController(_combinedRt);

        var sub = MapCoreToSubModule(selected.coreA) ?? MapCoreToSubModule(selected.coreB);

        _combinedCtl.Setup(new FireInitContext {
            runtime = _combinedRt,
            owner   = owner,
            pool    = poolManager,
            subCore = sub
        });
    }

    void Update()
    {
        if (!firePoint || !owner) return;
        float dt = Time.deltaTime;

        // 기본 무기 항상 발사 루프
        _defaultCtl?.TickAndFire(firePoint, _aimDir, owner, dt);

        // 선택된 조합 무기 병행 발사(있을 때만)
        _combinedCtl?.TickAndFire(combinedFirePoint, _aimDir, owner, dt);

        // 쿨타임 타이머 감소
        if (_switchTimer > 0f)
            _switchTimer -= dt;
    }


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
    public void SetAimDir(Vector2 dir)
    {
        if (dir.sqrMagnitude < 1e-6f) return;
        _aimDir = dir.normalized;
    }

    // --- 유틸: CoreSO → SubCore 매핑(키워드 기반 최소 구현) ---
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