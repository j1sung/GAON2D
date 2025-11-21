using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorController : MonoBehaviour
{
    public static SectorController Instance;
    public int aliveEnemies = 0;

    public Sector currentSector; // 현재 섹터
    public GameObject[] rewardPrefab; // 보상들

    private void Awake()
    {
        Instance = this;
    }

    public void SetCurrentSector(Sector sector)
    {
        currentSector = sector;

        if(currentSector.portal.Length == 0) // 포탈 없으면 마지막 섹터
        {
            return;
        }

        foreach (var obj in currentSector.portal)
            obj.SetActive(false);
        currentSector.rewardSpot.gameObject.SetActive(false);
    }

    public void OnEnemySpawned()
    {
        aliveEnemies++;
    }

    public void OnEnemyDied()
    {
        aliveEnemies--;
        if (aliveEnemies <= 0)
        {
            StageClear();
        }
    }

    void StageClear()
    {
        aliveEnemies = 0; // 적 카운트 초기화
        currentSector.rewardSpot.gameObject.SetActive(true); // 보상 활성화 (휙득 -> 둘 중 하나 선택)

        // 첫 번째 뽑기
        int index1 = Random.Range(0, rewardPrefab.Length);

        // 두 번째 뽑기 (첫 번째와 겹치지 않게)
        int index2;
        do
        {
            index2 = Random.Range(0, rewardPrefab.Length);
        }
        while (index2 == index1);

        // 생성
        Vector3 leftPos = currentSector.rewardSpot.GetChild(0).position;
        Vector3 rightPos = currentSector.rewardSpot.GetChild(1).position;

        Instantiate(rewardPrefab[index1], leftPos, Quaternion.identity);
        Instantiate(rewardPrefab[index2], rightPos, Quaternion.identity);

        foreach (var obj in currentSector.portal)
            obj.SetActive(true); // 포탈 활성화
    }
}
