using UnityEngine;

public class BossMapPatternController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Pool")]
    [SerializeField] private string hazardPoolName = "FallingHazard";
    [SerializeField] private FallingHazard hazardPrefab;

    [Header("Spawn")]
    [SerializeField] private float interval = 1.5f;
    //[SerializeField] private float spawnHeight = 8f;
    [SerializeField] private float randomX = 0.5f;

    // Todo : 추가 요소 (문제 생길 시 밑에 부분 수정).
    [Header("Telegraph")]
    [SerializeField] private string warningPoolName = "WarningCircle";
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private float warningDelay = 1.0f; // 원 표시 후 언제 떨어질지.
    [SerializeField] private float warningScale = 1.5f;
    // Todo : 따라다니다가 떨어질 때 고정하고 싶으면 밑에 변수 수정.
    [SerializeField] private bool followPlayerUntilDrop; 


    private float timer;

    private void Awake()
    {
        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector2 playerPos = player.position;
        Vector2 targetPos = new Vector2(
            playerPos.x + Random.Range(-randomX, randomX),
            playerPos.y
        );

        StartCoroutine(SpawnRoutine(targetPos));
    }

    private System.Collections.IEnumerator SpawnRoutine(Vector2 targetPos)
    {
        // 경고 원 풀에서 꺼내기.
        GameObject warning = MapPoolManager.instance.Get(warningPoolName, warningPrefab);
        
        if (warning != null)
        {
            // 바닥에 눕히기.
            warning.transform.position = targetPos;
            warning.transform.localScale = Vector3.zero;
        }

        // 딜레이 동안 경고 연출 (따라다니기는 일단 대기).
        float t = 0f;
        while (t < warningDelay)
        {
            t += Time.deltaTime;

            if (followPlayerUntilDrop)
            {
                // Todo : 따라다니되 x만 살짝 랜덤 유지하고 싶으면 여기서 로직 조정. 
                Vector2 p = player.position;
                targetPos = new Vector2(targetPos.x, p.y);
                if (warning != null)
                {
                    warning.transform.position = targetPos;
                }
            }

            // 0에서 warningScale로 커지기
            if (warning != null)
            {
                float p = Mathf.Clamp01(t /  warningDelay);
                warning.transform.localScale = Vector3.one * (warningScale * p);
            }

            yield return null;
        }

        // 3) 경고 원 반납.
        if (warning != null) MapPoolManager.instance.Release(warning);

        // 4) 낙하 오브젝트 풀에서 꺼내기.
        GameObject go = MapPoolManager.instance.Get(hazardPoolName, hazardPrefab.gameObject);
        if (go == null) yield break;

        // 떨어지는 시작 위치를 위로 올리기.
        //Vector2 spawnPos = targetPos + Vector2.up * spawnHeight;
        //go.transform.position = targetPos;

        // 5) FallingHazard에 "목표 지점" 전달해서 낙하 시작.
        var hazard = go.GetComponent<FallingHazard>();
        if (hazard == null)
        {
            MapPoolManager.instance.Release(go);
            yield break;
        }

        hazard.Spawn(targetPos, ReleaseHazard);
    }

    private void ReleaseHazard(FallingHazard hazard)
    {
        MapPoolManager.instance.Release(hazard.gameObject);
    }
}
