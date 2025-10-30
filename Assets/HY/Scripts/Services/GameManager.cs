using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameConfing config;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private SceneController sceneController;

    public GameState Current => _sm.State;
    public float TimeLeft => _timer?.TimeLeft ?? 0f;

    private StateMachine<GameState> _sm;
    private GameTimer _timer;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this; DontDestroyOnLoad(gameObject);

        _sm = new StateMachine<GameState>(GameState.Boot, OnStateChanged);
        _timer = new GameTimer(config.gameDurationSeconds);

        GameEvents.OnSelectionOpened += HandleSelectionOpen;
        GameEvents.OnSelectionClosed += HandleSelectionClose;
        GameEvents.OnRequestSceneChange += HandleSceneChange;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (_sm.State != GameState.Playing) return;
        if (_timer.Tick())
        {
            _sm.Change(GameState.GameOver);
            GameEvents.OnGameOver?.Invoke();
        }
    }

    public void StartGame()
    {
        _timer.Reset(config.gameDurationSeconds);
        _timer.Start();
        _sm.Change(GameState.Playing);
        GameEvents.OnGameStarted?.Invoke();
    }

    private void HandleSelectionOpen()
    {
        if (_sm.State != GameState.Playing) return;
        _sm.Change(GameState.LevelUpSelect);
        pauseManager.Pause(); 
    }
    private void HandleSelectionClose()
    { 
        if (_sm.State != GameState.LevelUpSelect) return;
        pauseManager.Resume();
        _sm.Change(GameState.Playing);
    }

    private void HandleSceneChange(string sceneName)
    {
        _sm.Change(GameState.Transition);
        pauseManager.Resume();
        sceneController.Load(sceneName);
    }

    private void OnStateChanged(GameState prev, GameState next)
    {
        // 상태별 진입/종료 훅 로깅/사운드 처리
        // Debug.Log($"State: {prev} -> {next}");
    }

    private void OnDestroy()
    {
        GameEvents.OnSelectionOpened -= HandleSelectionOpen;
        GameEvents.OnSelectionClosed -= HandleSelectionClose;
        GameEvents.OnRequestSceneChange -= HandleSceneChange;
    }
}
