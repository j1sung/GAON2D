/// 런타임 시점의 무기 데이터 수치를 보관하는 클래스.
/// WeaponSO는 불변 데이터이고, RuntimeWeapon은 플레이 중 수치 변화용이다.

using UnityEngine;

public class RuntimeWeapon
{
    // 원본 ScriptableObject 데이터 (불변)
    public WeaponSO source { get; private set; }
    // 실제 사용할 프리팹
    public GameObject bulletPrefabKey;

    // 런타임에서 조정 가능한 수치
    public float RUN_damage;          // 공격력
    public float RUN_fireRate;        // 연사 간격 (한 탄마다 몇 초)
    public float RUN_coolTime;        // 스킬 전체 쿨타임
    public int RUN_fireCount;        // 한 번의 스킬에서 발사되는 탄 개수
    public float RUN_projectileSpeed; // 탄속
    public float RUN_projectileLifetime;        // 탄 생존 시간
    public StatusTag RUN_OnHitTags;   // 피격 시 적용할 상태이상
    public ISubCoreModule[] subCore; // 산탄, 관통 등 추가 효과 체인

    public RuntimeWeapon(WeaponSO so)
    {
        source = so;

        bulletPrefabKey = so.bulletPrefab;

        // ScriptableObject의 기본값 복사
        RUN_damage = source.damage;
        RUN_fireRate = source.fireRate;
        RUN_coolTime = source.coolTime;
        RUN_fireCount = source.burstCount;
        RUN_projectileSpeed = source.projectileSpeed;
        RUN_projectileLifetime = source.projectileLifetime;
        RUN_OnHitTags = source.onHitTags;

        // 기본적으로 Modifier 없음
        subCore = System.Array.Empty<ISubCoreModule>();
    }

    /// 런타임 중 버프나 강화로 수치를 변화시킬 수 있음.
    /// Mathf.Max는 값이 너무 작아지는것을 방지
    public void AddDamage(float delta) => RUN_damage += delta;
    public void AddFireRate(float delta) => RUN_fireRate = Mathf.Max(0.05f, RUN_fireRate - delta);
    public void AddCooldown(float delta) => RUN_coolTime = Mathf.Max(0f, RUN_coolTime - delta);
}