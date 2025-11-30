using UnityEngine;

public class GameOverBanner : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    void OnEnable() { GameEvents.OnGameOver += Show; }
    void OnDisable() { GameEvents.OnGameOver -= Show; }
    void Show()
    {
        if (panel) panel.SetActive(true);
    }
}
