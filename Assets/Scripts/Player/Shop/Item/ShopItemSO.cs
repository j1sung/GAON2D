using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class ShopItemSO : ScriptableObject
{
    public string itemName;
    public string Description;
    public int price;

    public float value;         
    public ShopEffectSO effect;
}