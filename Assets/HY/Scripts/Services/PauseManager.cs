using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool IsPaused { get; private set; }

    public void Pause()
    {
        if (IsPaused) return;
        IsPaused = true;
        Time.timeScale = 0f; // 물리/업데이트 정지
        GameEvents.OnGamePaused?.Invoke();
    }
    public void Resume()
    {
        if (!IsPaused) return;
        IsPaused = false;
        Time.timeScale = 1f;
        GameEvents.OnGameResumed?.Invoke();
    }
}
