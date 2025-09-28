using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField] private GameConfing config;
    [SerializeField] private string overrideNextScene; 

    public string Prompt => $"[{config.interactKey}] ¿Ãµø";

    private bool _playerInside;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerTag>()) _playerInside = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerTag>()) _playerInside = false;
    }
    private void Update()
    {
        if (!_playerInside) return;
        if (Input.GetKeyDown(config.interactKey)) Interact();
    }
    public void Interact()
    {
        var next = string.IsNullOrEmpty(overrideNextScene) ? config.nextSceneName : overrideNextScene;
        GameEvents.OnRequestSceneChange?.Invoke(next);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center, GetComponent<Collider2D>().bounds.size);
    }
}
