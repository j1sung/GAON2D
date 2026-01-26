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
    [SerializeField] private float spawnHeight = 8f;
    [SerializeField] private float randomX = 0.5f;

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
        /*Vector2 spawnPos = new Vector2(
            playerPos.x + Random.Range(-randomX, randomX),
            playerPos.y + spawnHeight
        );*/
        Vector2 targetPos = new Vector2(
            playerPos.x + Random.Range(-randomX, randomX),
            playerPos.y
        );

        GameObject go = MapPoolManager.instance.Get(
            hazardPoolName,
            hazardPrefab.gameObject
        );

        //go.transform.position = spawnPos;
        go.transform.position = targetPos;

        var hazard = go.GetComponent<FallingHazard>();
        hazard.Spawn(targetPos, ReleaseHazard);
    }

    private void ReleaseHazard(FallingHazard hazard)
    {
        MapPoolManager.instance.Release(hazard.gameObject);
    }
}
