using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("UI References")]
    public Image[] weaponSlots;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image expBar;
    [SerializeField] private Text level;

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
        var ps = PlayerStatus.Instance;
        if (ps != null)
        {
            ps.OnHPChanged += UpdateHPBar;
            ps.OnExpChanged += UpdateExpBar;
            ps.OnLevelUp += UpdateLevel;

            // ???? ????
            UpdateHPBar(ps.currentHP / ps.RUN_maxHP);
            UpdateExpBar(ps.currentExp / ps.RUN_expToNextLevel);
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

    public void UpdateWeaponSlot(int slotIndex, WeaponSO weaponData)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length) return;

        var img = weaponSlots[slotIndex];
        img.sprite = weaponData.weaponIcon;
        img.color = Color.white;
    }
}