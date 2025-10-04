using UnityEngine;
using UnityEngine.UI; // TMP를 쓴다면 TMPro 네임스페이스로 변경

public class GameTimer : MonoBehaviour
{
    public Text timerText;  // UI Text 연결
    public float totalTime = 600f; // 10분 (600초)

    private float currentTime;

    void Start()
    {
        currentTime = totalTime;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            currentTime = 0;
            // 게임 종료 로직 추가 가능
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}