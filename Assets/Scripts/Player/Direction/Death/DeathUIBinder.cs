using UnityEngine;

public class DeathUIBinder : MonoBehaviour
{
    [SerializeField] private CanvasGroup deathCanvas;

    private void Start()
    {
        var player = FindObjectOfType<DeathDirection>();
        if (player != null)
        {
            player.BindDeathUI(deathCanvas);
        }
    }
}