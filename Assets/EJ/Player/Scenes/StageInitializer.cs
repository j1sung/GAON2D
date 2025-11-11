using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageInitializer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(BootAndStart());
    }

    private IEnumerator BootAndStart()
    {
        //    GameManager가 없을 수 있으니 한두 프레임 기다리며 탐색
        if (GameManager.Instance == null)
        {
            // 혹시 씬에 존재하지만 아직 Awake 중일 수 있으니 한 프레임 대기
            yield return null;

            if (GameManager.Instance == null)
            {
                Debug.LogWarning("[StageInitializer] GameManager가 없어 타이머 시작 불가. Main에서 실행하거나, GameManager가 DontDestroyOnLoad로 존재하는지 확인하세요.");
                yield break;
            }
        }

        //    (PauseManager가 씬 전환 전에 Pause 상태였을 수 있음)
        Time.timeScale = 1f;
        Debug.Log("[StageInitializer] Stage 씬에서 타이머 시작");
        GameManager.Instance.StartStage();
    }
}
