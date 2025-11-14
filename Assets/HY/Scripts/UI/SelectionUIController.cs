using UnityEngine;

public class SelectionUIController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private PlayerStatus playerStatus;

    private void Awake()
    {
        if (GameInstance.Instance != null && GameInstance.Instance.player != null)
        {
            playerStatus = GameInstance.Instance.player.GetComponent<PlayerStatus>();
        }
    }

    private void OnEnable()
    {
        if (playerStatus == null && GameInstance.Instance?.player != null)
        {
            playerStatus = GameInstance.Instance.player.GetComponent<PlayerStatus>();
        }

        if (playerStatus != null) playerStatus.OnLevelUp += OnLevelUp;

    }

    private void OnDisable()
    {
        if (playerStatus != null)
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

    public void OnLevelUp(int level) => Open();
    public void OnSelectOption() => Close();
}
