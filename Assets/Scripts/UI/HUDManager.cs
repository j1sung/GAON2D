using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("UI References")]
    public Image[] weaponSlots;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image expBar;
    [SerializeField] private Text level;
    [SerializeField] private TextMeshProUGUI Bits;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        var context = PlayerAgents.Instance.Context;
        var status = context.Status;
        var currency = context.Currency;

        if (context != null)
        {
            status.OnHPChanged += UpdateHPBar;
            status.OnExpChanged += UpdateExpBar;
            status.OnLevelUp += UpdateLevel;
            currency.OnBitsChanged += UpdateBit;

            UpdateHPBar(status.currentHP / status.RUN_maxHP);
            UpdateExpBar(status.currentExp / status.RUN_expToNextLevel);
            UpdateLevel();
        }
    }

    void UpdateHPBar(float normalizedHP)
    {
        if (hpBar != null)
            hpBar.fillAmount = normalizedHP;
    }

    void UpdateExpBar(float normalizedExp)
    {
        if (expBar != null)
            expBar.fillAmount = normalizedExp;
    }

    void UpdateLevel()
    {   
        var ps = PlayerStatus.Instance;
        
        if (ps != null)
        {
            level.text = $"Lv. {ps.level}";
        }
    }

    void UpdateBit(int bitAmount)
    {
        Bits.text = $"x {bitAmount}";
    } 

    public void UpdateWeaponSlot(int slotIndex, WeaponSO weaponData)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length) return;

        var img = weaponSlots[slotIndex];
        img.sprite = weaponData.weaponIcon;
        img.color = Color.white;
    }
}