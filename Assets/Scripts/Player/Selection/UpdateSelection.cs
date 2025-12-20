using UnityEngine;
using UnityEngine.UI;

public class UpdateSelection : MonoBehaviour
{   
    private SelectionManager _manager;
    private UpgradeSelectionSO _data;

    [SerializeField] private Text nameText;
    [SerializeField] private Image icon;
    [SerializeField] private Text descriptionText;

    public void SetData(UpgradeSelectionSO data)
    {   
        _data = data;
        nameText.text = data.selectionName;
        icon.sprite = data.selectionImage;
        descriptionText.text = data.selectionDescription;
    }

    public void BindManager(SelectionManager manager)
    {   
        _manager = manager;
    }

    public void OnClick()
    {
        _manager.OnSelect(_data);
    }
}
