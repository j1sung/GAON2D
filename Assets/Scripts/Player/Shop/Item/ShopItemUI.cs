using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI soldText;   // SOLD OUT 표시용
    [SerializeField] private Button buyButton;

    ShopItemSO _item;
    PlayerContext _ctx;

    bool _isSold;

    public void Setup(ShopItemSO item, PlayerContext ctx)
    {
        _item = item;
        _ctx = ctx;
        _isSold = false;

        nameText.text = _item.itemName;
        priceText.text = _item.price.ToString();

        if (soldText) soldText.gameObject.SetActive(false);

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnClickBuy);

        buyButton.interactable = true;
    }

    void OnClickBuy()
    {
        if (_isSold) return;
        if (_ctx == null || _item == null) return;

        bool success = _ctx.Currency.TrySpendBits(_item.price);

        if (!success)
        {
            ShowNotEnoughMessage();
            return;
        }

        // 구매 성공
        ApplyItem();
        MarkSoldOut();
    }

    void ApplyItem()
    {
        // 3단계에서 구현할 버프 적용
        Debug.Log($"{_item.itemName} 구매 성공");
    }

    void MarkSoldOut()
    {
        _isSold = true;
        buyButton.interactable = false;

        if (soldText)
            soldText.gameObject.SetActive(true);
    }

    void ShowNotEnoughMessage()
    {
        Debug.Log("코인 부족!");
        // 나중에 UI 팝업으로 교체
    }
}