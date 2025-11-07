using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("UI References")]
    public Image[] weaponSlots;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider expBar;
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
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.OnHPChanged += UpdateHPBar;
            PlayerStatus.Instance.OnExpChanged += UpdateExpBar;
            PlayerStatus.Instance.OnLevelUp += () => UpdateLevel(PlayerStatus.Instance.level);
            
        }
    }

    void UpdateHPBar(float normalizedHP)
    {
        if (hpBar != null)
            hpBar.value = normalizedHP;
    }

    void UpdateExpBar(float normalizedExp)
    {
        if (expBar != null)
            expBar.value = normalizedExp;
    }

    void UpdateLevel(int nextLevel)
    {
        if (level != null)
        {
            level.text = $"Lv. {nextLevel}";
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