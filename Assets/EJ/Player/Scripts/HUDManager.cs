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

    public void UpdateWeaponSlot(int slotIndex, Sprite weaponIcon)
    {
        if (slotIndex <= weaponSlots.Length)
        {
            weaponSlots[slotIndex - 1].sprite = weaponIcon;
            weaponSlots[slotIndex - 1].color = Color.white;
        }
    }
}