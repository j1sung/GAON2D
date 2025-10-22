using UnityEngine;

public class WeaponController
{
    private RuntimeWeapon _weapon;
    private IFireModule _module;

    // 스킬 타이머
    private float _coolTimer = 0f;

    // 연사 간격 타이머
    private float _fireTimer = 0f;

    // 현재 남은 공격 수
    private int _fireRemaining = 0;

    public WeaponController(RuntimeWeapon weapon)
    {
        _weapon = weapon;
        _module = _weapon.source.CreateModule();
    }

    public void Setup(FireInitContext ctx) => _module.Init(ctx);

    // WeaponManager.cs에서 매 프레임 호출.
    public void TickAndFire(Transform muzzle, Vector2 aimDir, Transform owner, float dt)
    {
        // 쿨타임 경과
        if (_coolTimer > 0f) _coolTimer -= dt;

        //  공격 Count가 남았다면 FireRate 간격으로 소진
        if (_fireRemaining > 0)
        {
            _fireTimer -= dt;

            // _fireTimer가 0 이하일 때마다 한 발씩 발사.
            // dt(Time.deltaTime)가 누적되어 _fireTimer가 음수가 되면, 아래 while 루프가 "밀린 탄환"을 모두 처리.
            while (_fireRemaining > 0 && _fireTimer <= 0f)
            {
                FireOnce(muzzle, aimDir, owner); // 한발 발사
                _fireRemaining--;

                // 다음 탄까지 fireRate만큼 누적
                _fireTimer += _weapon.RUN_fireRate;
            }

            // 공격이 끝났다면 이제부터 쿨타임 시작
            if (_fireRemaining == 0 && _coolTimer <= 0f)
                _coolTimer = _weapon.RUN_coolTime;
        }
        // 남은 공격이 없고 쿨타임이 지났다면 다시 공격 시작
        else if (_coolTimer <= 0f)
        {
            StartFire();
        }
    }

    /// 공격 시작
    private void StartFire()
    {
        _fireRemaining = Mathf.Max(1, _weapon.RUN_fireCount);
        _fireTimer = 0f; // 첫 탄 즉시 발사
    }

    /// 한 발 발사
    private void FireOnce(Transform p_muzzle, Vector2 p_aimDir, Transform p_owner)
    {
        if (p_aimDir.sqrMagnitude < 0.1f)
            p_aimDir = Vector2.right;

        var ctx = new FireContext
        {
            muzzle = p_muzzle,
            aimDir = p_aimDir.normalized,
            seed = Random.Range(0, 999999)
        };

        _module.Fire(ctx);
    }

    public RuntimeWeapon GetRuntime() => _weapon;
}
