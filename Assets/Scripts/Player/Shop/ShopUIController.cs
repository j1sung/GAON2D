using UnityEngine;

public sealed class ShopUIController : MonoBehaviour
{   
    [Header("refs")]
    [SerializeField] private GameObject _shopPanel;     
    [SerializeField] private bool pauseGame = true;
    private PlayerContext _ctx;

    [Header("Item")]
    [SerializeField] private ShopItemSO[] items;
    [SerializeField] private ShopItemUI[] itemSlots;


    public bool IsOpen { get; private set; }

    void Awake()
    {
        if (_shopPanel) _shopPanel.SetActive(false);
        IsOpen = false;
    }

    public void Open()
    {
        if (!_shopPanel || IsOpen) return;

        _shopPanel.SetActive(true);
        IsOpen = true;

        // 아이템 세팅
        InitItems(_ctx);

        if (pauseGame) Time.timeScale = 0f;
    }

    public void Close()
    {
        if (!_shopPanel || !IsOpen) return;

        if (pauseGame) Time.timeScale = 1f;

        _shopPanel.SetActive(false);
        IsOpen = false;
    }

    public void Bind(PlayerContext ctx)
    {
        _ctx = ctx;
    }

    public void InitItems(PlayerContext ctx)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i >= items.Length) break;

            itemSlots[i].Setup(items[i], ctx);
        }
    }
}