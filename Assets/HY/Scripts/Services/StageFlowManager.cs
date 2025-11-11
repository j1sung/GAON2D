using UnityEngine;

public class StageFlowManager : MonoBehaviour
{
    public static StageFlowManager Instance { get; private set; }

    [Header("Config (ScriptableObject)")]
    [SerializeField] private StageFlowData data;

    private int currentStageIndex = -1;
    private bool inBossRoom = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (data == null)
            Debug.LogError("StageFlowData가 비어 있습니다!");
    }

    public void ResetFlow()
    {
        currentStageIndex = -1;
        inBossRoom = false;
        Debug.Log("진행 상태 리셋");
    }

    public string GetLobbyScene() => data != null ? data.lobbyScene : null;

    public string GetNextScene()
    {
        if (data == null || data.stages == null || data.stages.Count == 0)
        {
            Debug.LogWarning("스테이지 데이터가 비어 있음");
            return null;
        }

        // 첫 진입: Stage1
        if (currentStageIndex == -1)
        {
            currentStageIndex = 0;
            inBossRoom = false;
            var first = data.stages[0].stageSceneName;
            Debug.Log($"첫 스테이지 시작: {first}");
            return first;
        }

        // 안전 범위 체크
        if (currentStageIndex < 0 || currentStageIndex >= data.stages.Count)
        {
            Debug.LogWarning($"인덱스 범위 초과: {currentStageIndex}/{data.stages.Count}");
            return null;
        }

        // 스테이지 → 보스
        if (!inBossRoom)
        {
            inBossRoom = true;
            var boss = data.stages[currentStageIndex].bossSceneName;
            Debug.Log($"보스방 진입: {boss}");
            return boss;
        }

        // 보스 → 다음 스테이지
        inBossRoom = false;
        currentStageIndex++;

        if (currentStageIndex >= data.stages.Count)
        {
            Debug.Log(" 모든 스테이지 클리어!");
            return null;
        }

        var next = data.stages[currentStageIndex].stageSceneName;
        Debug.Log($" 다음 스테이지 진입: {next}");
        return next;
    }
}