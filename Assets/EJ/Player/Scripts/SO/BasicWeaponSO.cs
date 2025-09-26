using UnityEngine;

[CreateAssetMenu(menuName = "SO/BasicWeapon")]
public class BasicWeaponSO : ScriptableObject
{
    [Header("Info")]
    public string weaponName;
    public Sprite weaponIcon;

    [Header("Recipe")]
    public BlueprintSO blueprint;
    public CoreSO coreA;
    public CoreSO coreB;

    [Header("Attack")] // 일단 디버그만
    public float damage;
    public float cooldown = 0.5f; // 디버그용 발사 주기

}
