using EnemyOwnedStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    int spawnCount = 0;
    int spawnIndex = 0;

    float time;
    [SerializeField] float spawnTime; // 스폰되는 시간

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        // 스폰 쿨타임이 돌면 스폰함
        time += Time.deltaTime;
        if(time > spawnTime) // 적마다 다른 스폰 타임 설정 필요!!
        {
            time = 0f;
            Spawn();
        }
    }
    void Spawn() // 추후 GameManager의 시간, 이벤트, 조건에 따라 적 스폰으로 수정
    {
        spawnCount++;
        //spawnIndex = spawnCount % 3 == 0 ?  1 : 0;

        GameObject enemyObj = GameInstance.Instance.pool.Get(spawnIndex);
        if (enemyObj == null)
        {
            // 최대치 도달로 인해 생성 실패
            return;
        }

        enemyObj.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // 적의 소환 위치 랜덤 포인트 지정... Range 0은 Spawner 클래스 위치라 뺌
    }
}
