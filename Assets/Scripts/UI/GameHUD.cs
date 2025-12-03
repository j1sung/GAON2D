using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private CanvasGroup pauseIcon; 

    private void OnEnable()
    {
        GameEvents.OnGamePaused += ShowPause;
        GameEvents.OnGameResumed += HidePause;
    }
    private void OnDisable()
    {
        GameEvents.OnGamePaused -= ShowPause;
        GameEvents.OnGameResumed -= HidePause;
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        float t = GameManager.Instance.TimeLeft;
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        timeText.text = $"{m:00}:{s:00}";
    }

    private void ShowPause() { pauseIcon.alpha = 1f; }
    private void HidePause() { pauseIcon.alpha = 0f; }
}
