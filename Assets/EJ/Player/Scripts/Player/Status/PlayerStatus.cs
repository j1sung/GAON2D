using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO baseStats;

    [Header("Runtime Base Stat")]
    public float RUN_maxHP;
    public float currentHP;
    public float RUN_baseDamage;
    public float RUN_damageMul;
    public float RUN_fireRateMul;
    public float RUN_critical;
    public float RUN_criticalMul;
    public float RUN_expCollectRadius;

    [Header("Level / EXP")]
    public int level = 1;
    public float currentExp = 0f;
    public float RUN_expToNextLevel;


    [Header("Move")]
    public float RUN_moveSpeed;
    public float RUN_dashSpeed;
    public float RUN_dashDuration;
    public float RUN_dashCooldown;

    public event System.Action OnLevelUp; // 레벨업시 발생하는 이벤트

    void Awake()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        if (baseStats == null) return;
        RUN_maxHP = baseStats.maxHP;
        currentHP = RUN_maxHP;
        RUN_baseDamage = baseStats.baseDamage;
        RUN_damageMul = baseStats.damageMul;
        RUN_fireRateMul = baseStats.fireRateMul;
        RUN_critical = baseStats.critical;
        RUN_criticalMul = baseStats.criticalMul;
        RUN_expToNextLevel = baseStats.expToNextLevel;
        RUN_expCollectRadius = baseStats.expCollectRadius;

        RUN_moveSpeed = baseStats.moveSpeed;
        RUN_dashSpeed = baseStats.dashSpeed;
        RUN_dashDuration = baseStats.dashDuration;
        RUN_dashCooldown = baseStats.dashCooldown;
    }

    // 최종 데미지 계산
    public float GetFinalDamage(float weaponDamage)
    {
        float total = (RUN_baseDamage + weaponDamage) * RUN_damageMul;
        bool isCrit = Random.value < RUN_critical;
        if (isCrit) total *= RUN_criticalMul;
        return total;
    }

    public void GainExp(float amount)
    {
        currentExp += amount;
        Debug.Log($"경험치 {currentExp}");
        while (currentExp >= RUN_expToNextLevel)
        {
            currentExp -= RUN_expToNextLevel;
            LevelUp();
        }
    }
    
    private void LevelUp()
    {
        level++;
        OnLevelUp?.Invoke();
    }
}
