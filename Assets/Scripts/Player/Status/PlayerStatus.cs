using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerStatus : MonoBehaviour, IDamageable
{   
    public static PlayerStatus Instance { get; private set; }

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
    public int level;
    public float currentExp;
    public float RUN_expToNextLevel;

    [Header("Move")]
    public float RUN_moveSpeed;
    public float RUN_dashSpeed;
    public float RUN_dashDuration;
    public float RUN_dashCooldown;
    
    public event Action<float> OnHPChanged; // 체력 변동
    public event Action<float> OnExpChanged; // 경험치 변동
    public event Action OnLevelUp; // 레벨업시 발생하는 이벤트
    public event Action OnDeath; // 사망시 발생하는 이벤트
    public event Action OnStatsChanged; // 스탯 변동

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        if (baseStats == null) return;
        RUN_maxHP = baseStats.maxHP;
        currentHP = RUN_maxHP;
        level = 1;
        currentExp = 0;
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

        OnHPChanged?.Invoke(currentHP / RUN_maxHP);
        OnExpChanged?.Invoke(currentExp / RUN_expToNextLevel);
    }

    // 최종 데미지 계산
    public float GetFinalDamage(float weaponDamage)
    {
        float total = (RUN_baseDamage + weaponDamage) * RUN_damageMul;
        bool isCrit = UnityEngine.Random.value < RUN_critical;
        if (isCrit) total *= RUN_criticalMul;
        return total;
    }

    public void ApplyHit(HitContext ctx)
    {
        if (currentHP <= 0) return;
        TakeDamage(ctx.damage);
        // 상태이상은 여기서 적용 가능(적도 마찬가지)
    }

    public void TakeDamage(float dmg)
    {
        currentHP = Mathf.Max(0, currentHP - dmg);
        OnHPChanged?.Invoke(currentHP / RUN_maxHP);

        if (currentHP <= 0)
            Die();
    }

    public void Die()
    {
        OnDeath?.Invoke();
    }

    // 경험치 획득
    public void GainExp(float amount)
    {
        currentExp += amount;
        OnExpChanged?.Invoke(currentExp / RUN_expToNextLevel);
        while (currentExp >= RUN_expToNextLevel)
        {
            currentExp -= RUN_expToNextLevel;
            LevelUp();
        }
        OnExpChanged?.Invoke(currentExp / RUN_expToNextLevel);
    }
    
    private void LevelUp()
    {
        level++;
        OnLevelUp?.Invoke();
    }

    public void StatsModifier(StatKind kind, float addValue, float mulValue = 1f) // 스탯 변화 메서드
    {
        switch (kind)
        {
            case StatKind.HP:
            RUN_maxHP += addValue;
            break;

            case StatKind.Damage:
            RUN_baseDamage += addValue;
            break;

            case StatKind.FireRate:
            RUN_fireRateMul *= mulValue;
            break;

            case StatKind.Critical:
            RUN_critical += addValue;
            break;  

            case StatKind.CriticalDamage:
            RUN_criticalMul *= addValue;
            break;   

            case StatKind.MoveSpeed:
            RUN_moveSpeed += addValue;
            break;                  
        }
        OnStatsChanged?.Invoke(); // HUD에 바로 상태반영 하기 위해 이벤트 발생
    }
}
