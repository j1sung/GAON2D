using UnityEngine;

public sealed class WeaponChange : MonoBehaviour
{
    [SerializeField] WeaponManager weaponManager; // WeaponManager로 변경

    void Update()
    {
        if (!weaponManager) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            weaponManager.Select(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            weaponManager.Select(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            weaponManager.Select(2);
    }
}