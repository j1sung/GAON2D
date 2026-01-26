using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField] private GameConfing config;
    [SerializeField] private string overrideNextScene;

    private bool _playerInside;
    public string Prompt => $"[{config.interactKey}] 이동";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _playerInside = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _playerInside = false;
    }

    private void Update()
    {
        if (_playerInside && Input.GetKeyDown(config.interactKey)) Interact();
        else
            Debug.Log(_playerInside);
    }

    public void Interact()
    {
        string nextScene = !string.IsNullOrEmpty(overrideNextScene)
            ? overrideNextScene
            : StageFlowManager.Instance?.GetNextScene();

        if (string.IsNullOrEmpty(nextScene))
        {
            Debug.Log("[Portal] 모든 스테이지 클리어! 엔딩/결과로 이동");
            GameEvents.OnGameOver?.Invoke();
            return;
        }

        Debug.Log($"[Portal] 이동 요청: {nextScene}");
        GameStateContext.IsStageTransition = true;

        GameEvents.OnRequestSceneChange?.Invoke(nextScene);


    }
}

public static class GameStateContext
{
    public static bool IsStageTransition = false;
}