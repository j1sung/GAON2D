public class PlayerContext
{
    public PlayerStatus Status { get; }
    public Inventory Inventory { get; }
    public WeaponManager WeaponManager { get; }

    public PlayerContext(
        PlayerStatus status,
        Inventory inventory,
        WeaponManager weaponManager)
    {
        Status        = status;
        Inventory     = inventory;
        WeaponManager = weaponManager;
    }
}