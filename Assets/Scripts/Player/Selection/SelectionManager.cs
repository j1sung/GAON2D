using UnityEngine;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{   
    public SelectionManager Instance;
    private const int SLOT_COUNT = 3;

    private PlayerContext _ctx;

    [SerializeField] private GameObject selectionPanelUI;
    [SerializeField] private SelectionDB selectionDB;
    [SerializeField] private UpdateSelection[] slots;

    void Awake()
    {   
        // SelectionManager을 각 슬롯 컴포넌트에 바인딩(OnSelect를 호출하기 위함)
        for (int i=0; i < slots.Length; i++) 
            slots[i].BindManager(this);
    }

    void Start()
    {   
        _ctx = PlayerAgents.Instance.Context;
        if (_ctx != null) _ctx.Status.OnLevelUp += OpenSelection;
    }

    void OnDisable()
    {
        _ctx.Status.OnLevelUp -= OpenSelection;
    }
    
    public void OnSelect(UpgradeSelectionSO data)
    {
        data.effect.ApplyEffect(_ctx);
    }

    private void OpenSelection()
    {
        // 패널 켜기
        selectionPanelUI.SetActive(true);
        GameEvents.OnSelectionOpened?.Invoke();

        var selections = PickRandomSelections();

        for (int i = 0; i < selections.Length; i++)
        {
            slots[i].gameObject.SetActive(true);
            ApplySelection(slots[i], selections[i]);
        }

        for (int i = selections.Length; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
    }

    private UpgradeSelectionSO[] PickRandomSelections()
    {
        var source = selectionDB.selectionList;
        int count = Mathf.Min(SLOT_COUNT, source.Count);

        List<UpgradeSelectionSO> temp = new List<UpgradeSelectionSO>(source);
        UpgradeSelectionSO[] result = new UpgradeSelectionSO[count];

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, temp.Count);
            result[i] = temp[index];
            temp.RemoveAt(index);
        }

        return result;
    }

    private void ApplySelection(UpdateSelection slot, UpgradeSelectionSO data)
    {
        slot.SetData(data);
    }

    // 버튼에서 호출용 (선택 완료)
    public void CloseSelection()
    {
        selectionPanelUI.SetActive(false);
        GameEvents.OnSelectionClosed?.Invoke();
    }
}