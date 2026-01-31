using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField] private GameConfing config;
    [SerializeField] private string overrideNextScene;

    private bool _playerInside;
    public string Prompt => $"[{config.interactKey}] �̵�";

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
    }

    public void Interact()
    {
        string nextScene = !string.IsNullOrEmpty(overrideNextScene)
            ? overrideNextScene
            : StageFlowManager.Instance?.GetNextScene();

        if (string.IsNullOrEmpty(nextScene))
        {
            Debug.Log("[Portal] ��� �������� Ŭ����! ����/����� �̵�");
            GameEvents.OnGameOver?.Invoke();
            return;
        }

        Debug.Log($"[Portal] �̵� ��û: {nextScene}");
        GameStateContext.IsStageTransition = true;

        GameEvents.OnRequestSceneChange?.Invoke(nextScene);


    }
}

public static class GameStateContext
{
    public static bool IsStageTransition = false;
}