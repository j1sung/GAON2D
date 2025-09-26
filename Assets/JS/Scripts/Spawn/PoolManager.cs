using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩들 저장하는 변수
    public GameObject[] prefabs;

    public int maxPoolSizes = 25; // 각 몬스터 풀의 최대 개수 -> 추후 몬스터마다 설정 필요할 수도?

    // 풀 담당 리스트들
    List<GameObject>[] pools;
    Transform[] poolGroups; // PoolManager 하위 그룹 오브젝트 만들기

    private void Awake()
    {
        // 모든 오브젝트 풀 리스트 초기화
        pools = new List<GameObject>[prefabs.Length];
        poolGroups = new Transform[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();

            // 프리펩 이름으로 그룹 오브젝트 생성
            GameObject group = new GameObject(prefabs[i].name + "Pool");
            group.transform.parent = transform;
            poolGroups[i] = group.transform;
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 비활성화 된 게임 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            // 발견하면 select 변수에 할당
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // 못 찾았다면?
        if (!select && pools[index].Count < maxPoolSizes)
        {
            // 새롭게 생성하고 select 변수에 할당(해당 프리펩 그룹 오브젝트 하위로 생성)
            select = Instantiate(prefabs[index], poolGroups[index]);
            pools[index].Add(select);
        }

        return select;
    }
}
