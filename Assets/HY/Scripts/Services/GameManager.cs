using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameConfing config;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private StageFlowManager stageFlowManager;

    private StateMachine<GameState> _sm;
    private GameTimer _timer;

    public GameState Current => _sm.State;
    public float TimeLeft => _timer?.TimeLeft ?? 0f;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _sm = new StateMachine<GameState>(GameState.Boot, OnStateChanged);
        _timer = new GameTimer(config.gameDurationSeconds);

        GameEvents.OnSelectionOpened += HandleSelectionOpen;
        GameEvents.OnSelectionClosed += HandleSelectionClose;
        GameEvents.OnRequestSceneChange += HandleSceneChange;
    }

    private void Start()
    {
        _sm.Change(GameState.Boot);
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

    public void StartStage()
    {
        _timer.Reset(config.gameDurationSeconds);
        _timer.Start();
        _sm.Change(GameState.Playing);
        GameEvents.OnGameStarted?.Invoke();

        string nextScene = stageFlowManager.GetNextScene();
        if (nextScene != null)
            sceneController.Load(nextScene);
        else
        {
            _sm.Change(GameState.GameOver);
            GameEvents.OnGameOver?.Invoke();
        }
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
        pauseManager?.Resume();

        if (sceneName == StageFlowManager.Instance?.GetLobbyScene())
            StageFlowManager.Instance.ResetFlow();
        sceneController.Load(sceneName);

    }

    private void OnStateChanged(GameState prev, GameState next)
    {
        Debug.Log($"[GameState] {prev} → {next}");
    }

    private void OnDestroy()
    {
        GameEvents.OnSelectionOpened -= HandleSelectionOpen;
        GameEvents.OnSelectionClosed -= HandleSelectionClose;
        GameEvents.OnRequestSceneChange -= HandleSceneChange;
    }
}
