using UnityEngine;

public class SelectionUIController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private PlayerStatus playerStatus;

    // 활성화 시 구독
    void OnEnable()
    {
        playerStatus.OnLevelUp += OnLevelUp;
    }

    void OnDisable()
    {
        playerStatus.OnLevelUp -= OnLevelUp;
    }

    public void Open()
    {
        panel.SetActive(true);
        GameEvents.OnSelectionOpened?.Invoke();
    }
    public void Close()
    {
        panel.SetActive(false);
        GameEvents.OnSelectionClosed?.Invoke();
    }

    public void OnLevelUp() => Open();
    public void OnSelectOption() => Close();
}
