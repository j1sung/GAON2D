using EnemyStateSpace;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    int spawnCount = 0;
    [SerializeField] int spawnIndex = 1;
    [SerializeField] int maxTotalEnemies;

    float time;
    //[SerializeField] float spawnTime; // 스폰되는 시간

    public EnemyPoolManager pool;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
    }

    private void Start()
    {
        Spawn();
    }

    /*
    void Update()
    {
        // 스폰 쿨타임이 돌면 스폰함
        time += Time.deltaTime;
        if(time > spawnTime) // 적마다 다른 스폰 타임 설정 필요!!
        {
            time = 0f;
            Spawn();
        }
    }*/

    void Spawn() // 추후 GameManager의 시간, 이벤트, 조건에 따라 적 스폰으로 수정
    {
        while (true)
        {
            if (spawnCount >= maxTotalEnemies)
            {
                return;
            }
            
            SectorController.Instance.OnEnemySpawned(); // 섹터 적 스폰 카운트

            // 1. 먼저 카운트 1 증가.
            spawnCount++;

            // 2. 적 생성 종류 선택 로직 -> 3번째 생성마다 근거리 적 생성 (추후 로직은 변경 가능!).
            spawnIndex = spawnCount % 3 == 0 ? 1 : 0;

            // 3. 해당 인덱스 적 풀링 생성.
            GameObject enemyObj = pool.Get(spawnIndex);

            // 4. 적 스폰 위치 순서대로 배정.
            enemyObj.transform.position = spawnPoint[spawnCount-1].position; 
        }
    }
}
