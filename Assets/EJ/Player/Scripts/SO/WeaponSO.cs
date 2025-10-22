using UnityEngine;
using Attack.Pooling;

[CreateAssetMenu(menuName = "SO/Weapon")]
public class WeaponSO : ScriptableObject
{
    public enum FireType { gun, Laser }

    [Header("Info")]
    public string weaponName;
    public Sprite weaponIcon;
    public FireType firetype;
    public GameObject bulletPrefab;  // 탄환 프리팹

    [Header("Recipe")]
    public BlueprintSO blueprint;
    public CoreSO coreA;
    public CoreSO coreB;

    [Header("기본 수치")] // 일단 디버그만
    public float damage;          // 공격력
    public float fireRate;        // 연사 간격 (한 탄마다 몇 초)
    public float coolTime;        // 스킬 전체 쿨타임
    public int burstCount;        // 한 번의 스킬에서 발사되는 탄 개수
    public float projectileSpeed; // 탄속
    public float projectileLifetime;  // 탄 생존 시간
    public StatusTag onHitTags; // 추가 효과(메인 코어)

    [Header("풀 키 이름")]
    public string bulletKey; // 탄환 pool key
    public string impactKey; // 피격 이펙트 pool key

    // 🔹 발사 방식 모듈 생성
    public IFireModule CreateModule()
    {
        switch (firetype)
        {
            case FireType.gun:
                return new BulletFireModule();
            default:
                return null;
        }
    }
}
