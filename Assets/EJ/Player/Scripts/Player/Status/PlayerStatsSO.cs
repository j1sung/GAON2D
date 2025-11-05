using UnityEngine;

[CreateAssetMenu(menuName = "SO/PlayerStat")]
public class PlayerStatsSO  : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHP = 100f;
    public float expToNextLevel = 50f;
    public float baseDamage = 10f;
    public float damageMul = 1f; // 데미지 배율. 
    public float fireRateMul = 1f; // 공격속도 배율.
    public float critical = 1f;
    public float criticalMul = 0.02f; // 크리티컬 시, 기본 2배 데미지.
    public float expCollectRadius = 3f;

    [Header("Move")]
    public float moveSpeed = 5f; // 이동속도
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
}
