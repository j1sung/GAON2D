using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon")]
public class WeaponSO : ScriptableObject
{
    public enum WeaponAttackType { gun, sword }

    [Header("Info")]
    public string weaponName;
    public Sprite weaponIcon;

    [Header("Recipe")]
    public BlueprintSO blueprint;
    public CoreSO coreA;
    public CoreSO coreB;

    [Header("Attack")] // 일단 디버그만
    public WeaponAttackType attackType = WeaponAttackType.gun;
    public float cooldown = 0.5f; // 디버그용 발사 주기

}
