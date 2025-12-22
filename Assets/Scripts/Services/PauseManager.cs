using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void Pause()
    {
        Debug.Log("�Ͻ����� ����");
        if (IsPaused) return;
        IsPaused = true;
        Time.timeScale = 0f; // ����/������Ʈ ����
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
