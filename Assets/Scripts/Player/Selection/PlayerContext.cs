public class PlayerContext
{
    public PlayerStatus Status { get; }
    public Inventory Inventory { get; }
    public WeaponManager WeaponManager { get; }
    public PlayerCurrency Currency { get; }

    public PlayerContext(
        PlayerStatus status,
        Inventory inventory,
        WeaponManager weaponManager,
        PlayerCurrency currency)
    {
        Status        = status;
        Inventory     = inventory;
        WeaponManager = weaponManager;
        Currency = currency;
    }
}