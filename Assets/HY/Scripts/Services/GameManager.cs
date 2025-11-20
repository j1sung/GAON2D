using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameConfing config;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private StageFlowManager stageFlowManager;
    [SerializeField] private StageFlowData stageFlowData;

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

        Debug.Log($"[BGM DEBUG] Scene Change Detected: {sceneName}");
        Debug.Log($"[BGM DEBUG] Lobby Scene Name: {StageFlowManager.Instance.GetLobbyScene()}");

        _sm.Change(GameState.Transition);
        pauseManager?.Resume();

        if (sceneName == StageFlowManager.Instance?.GetLobbyScene())
        {
            StageFlowManager.Instance.ResetFlow();
            Debug.Log("[GameManager] 로비 진입 — 진행 상태 초기화");
        }

        sceneController.Load(sceneName);

        // 씬 로드 후 자동으로 Stage 타이머 시작 감지 코루틴 실행
        //StartCoroutine(WaitForSceneAndStart(sceneName));

        StartCoroutine(WaitAndPlayBGM(sceneName));
    }

    private IEnumerator WaitForSceneAndStart(string sceneName)
    {
        // 씬이 완전히 로드될 때까지 잠시 대기
        yield return new WaitForSeconds(0.2f);

        //  Lobby에서는 타이머 시작 안 함
        if (sceneName == StageFlowManager.Instance.GetLobbyScene())
            yield break;

        // Stage1, Stage2 … 이런 이름의 씬이면 자동으로 타이머 시작
        if (sceneName.ToLower().Contains("stage"))
        {
            Debug.Log($"[GameManager] {sceneName} 진입 — 타이머 자동 시작");
            StartStage();
        }
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

    private IEnumerator WaitAndPlayBGM(string sceneName)
    {

        Debug.Log("[BGM DEBUG] WaitAndPlayBGM 실행됨");


        yield return new WaitForSeconds(0.2f);

        if (sceneName == StageFlowManager.Instance.GetLobbyScene())
        {
            AudioManager.Instance.PlayBGM(stageFlowData.bgm);
            yield break;
        }

        if (sceneName.ToLower().Contains("stage"))
        {
            var stageInfo = StageFlowManager.Instance.GetCurrentStageInfo();
            if (stageInfo != null && stageInfo.bgm != null)
            {
                AudioManager.Instance.PlayBGM(stageInfo.bgm);
            }
        }
    }
}

