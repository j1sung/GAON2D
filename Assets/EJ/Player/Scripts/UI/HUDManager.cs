using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    public Image[] weaponSlots;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void UpdateWeaponSlot(int slotIndex, WeaponSO weaponData)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length) return;

        var img = weaponSlots[slotIndex];
        img.sprite = weaponData.weaponIcon;
        img.color = Color.white;
    }
}