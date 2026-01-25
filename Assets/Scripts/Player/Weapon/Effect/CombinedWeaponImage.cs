using UnityEngine;

public sealed class CombinedWeaponImage : MonoBehaviour
{
    [SerializeField] private Inventory inventory;          // 플레이어 인벤토리
    [SerializeField] private SpriteRenderer iconRenderer;  // CombinedWeapon의 SpriteRenderer

    void Awake()
    {
        if (!inventory) inventory = FindObjectOfType<Inventory>();
        UpdateVisual();
    }

    void OnEnable()
    {
        if (inventory != null)
            inventory.OnWeaponsChanged += UpdateVisual;
    }

    void OnDisable()
    {
        if (inventory != null)
            inventory.OnWeaponsChanged -= UpdateVisual;
    }

    private void UpdateVisual()
    {
        if (iconRenderer == null) return;

        var selected = inventory ? inventory.GetSelectedCombinedWeapon() : null;

        if (selected == null || selected.weaponIcon == null)
        {
            iconRenderer.enabled = false;  // 조합 무기 없음 → 숨김
        }
        else
        {
            iconRenderer.enabled = true;
            iconRenderer.sprite = selected.weaponIcon; // 조합 무기 이미지 적용
        }
    }
}