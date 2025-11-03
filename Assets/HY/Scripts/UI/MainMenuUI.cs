using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private string lobbySceneName = "Lobby";

    private void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);
        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitClicked);
    }

    public void OnStartClicked()
    {
        Debug.Log("[MainMenuUI] Start ¹öÆ° Å¬¸¯µÊ");
        SceneController.Instance?.Load(lobbySceneName);
    }

    public void OnExitClicked()
    {
        Debug.Log("[MainMenuUI] Exit ¹öÆ° Å¬¸¯µÊ");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
