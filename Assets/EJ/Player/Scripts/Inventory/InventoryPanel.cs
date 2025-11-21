using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    public static InventoryPanel Instance { get; private set; }

    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text fireRateText;
    [SerializeField] private TMP_Text critText;
    [SerializeField] private TMP_Text critDamageText;
    [SerializeField] private TMP_Text speedText;

    [Header("Slots")]
    [SerializeField] private Image[] blueprintSlots;
    [SerializeField] private Image[] coreA_Slots;
    [SerializeField] private Image[] coreB_Slots;

    [SerializeField] private GameObject invPanel;
    [SerializeField] private bool isOpened;


    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        var st = PlayerStatus.Instance;
        var inv = FindObjectOfType<Inventory>();

        st.OnStatsChanged += () => UpdateStatusUI(st);
        inv.OnInventoryChanged += () => UpdateSlotUI(inv);

        // 초기 업데이트
        UpdateStatusUI(st);
        UpdateSlotUI(inv);
    }
    public void UpdateStatusUI(PlayerStatus st)
    {
        damageText.text = $"Damage: {st.RUN_damageMul:F1}";
        fireRateText.text = $"Fire Rate: {st.RUN_fireRateMul:F2}";
        critText.text = $"Critical: {st.RUN_critical}%";
        critDamageText.text = $"Crit DMG: {st.RUN_criticalMul:F1}";
        speedText.text = $"Move Speed: {st.RUN_moveSpeed:F1}";
    }

    public void OpenInventory()
    {   
        if (invPanel == null) return;
        if (isOpened != true)
        {   
            invPanel.SetActive(true);
            isOpened = true;
        }
        else
        {   
            invPanel.SetActive(false);
            isOpened = false;
        }
    }

    public void UpdateSlotUI(Inventory inv)
    {
        for (int i = 0; i < inv.slots.Length; i++)
        {
            var s = inv.slots[i];

            // Blueprint
            if (s.blueprint == null)
            {
                blueprintSlots[i].enabled = false;
            }
            else
            {
                blueprintSlots[i].enabled = true;
                blueprintSlots[i].sprite = s.blueprint.image;
            }

            // Core A
            if (s.Cores.Count > 0 && s.Cores[0] != null)
            {
                coreA_Slots[i].enabled = true;
                coreA_Slots[i].sprite = s.Cores[0].image;
            }
            else
            {
                coreA_Slots[i].enabled = false;
            }

            // Core B
            if (s.Cores.Count > 1 && s.Cores[1] != null)
            {
                coreB_Slots[i].enabled = true;
                coreB_Slots[i].sprite = s.Cores[1].image;
            }
            else
            {
                coreB_Slots[i].enabled = false;
            }
        }
    }
}