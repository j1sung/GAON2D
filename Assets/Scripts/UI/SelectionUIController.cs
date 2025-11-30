using UnityEngine;

public class SelectionUIController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private PlayerStatus playerStatus;

    private void Awake()
    {
        BindPlayerStatus();
    }

    private void BindPlayerStatus()
    {
        if (playerStatus != null) return;

        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerStatus = playerObj.GetComponent<PlayerStatus>();
            playerStatus.OnLevelUp += OnLevelUp;
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
        Debug.Log("일시정지 실행");
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
