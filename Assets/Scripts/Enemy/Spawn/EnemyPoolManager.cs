using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    // 프리펩들 저장하는 변수
    public GameObject[] prefabs;

    public int maxPoolSizes = 10; // 각 몬스터 풀의 최대 개수 -> 추후 몬스터마다 설정 필요할 수도?
    private int[] createdCounts; // 종류별 생성 갯수 제한 비교

    // Inactive 스택: index => 비활성 오브젝트들
    private Stack<GameObject>[] inactiveStacks;

    // reverse: instance -> index (반납 시 어디로 push할지)
    private Dictionary<GameObject, int> reverse;

    // PoolManager 하위 그룹 오브젝트(카테고리) 만들기
    private Transform[] poolGroups; 

    private void Awake()
    {
        int n = prefabs.Length;
        // 모든 오브젝트 풀 리스트 초기화
        inactiveStacks = new Stack<GameObject>[n];
        reverse = new Dictionary<GameObject, int>(n*maxPoolSizes);
        createdCounts = new int[n];
        poolGroups = new Transform[n];

        for (int i = 0; i < n; i++)
        {
            inactiveStacks[i] = new Stack<GameObject>(maxPoolSizes);

            // 프리펩 이름으로 그룹 오브젝트 생성
            GameObject group = new GameObject(prefabs[i].name + "Pool");
            group.transform.SetParent(transform, false);
            poolGroups[i] = group.transform;
        }
    }

    public GameObject Get(int index)
    {
        if(index < 0 || index >= prefabs.Length)
        {
            Debug.Log($"[EnemyPoolManager] 잘못된 index: {index}");
            return null;
        }

        GameObject select = null;

        // 선택한 적 풀의 비활성화 된 게임 오브젝트 꺼내기 (O(1))
        if(inactiveStacks[index].Count > 0)
        {
            select = inactiveStacks[index].Pop();
        }
        else
        {
            if (createdCounts[index] >= maxPoolSizes)
            {
                return null;
            }
            select = Instantiate(prefabs[index], poolGroups[index]);
            createdCounts[index]++;
            reverse[select] = index;
        }
        select.SetActive(true);

        return select;
    }

    public void Release(GameObject instance)
    {
        if(instance == null) return;

        if (!reverse.TryGetValue(instance, out int index))
        {
            Destroy(instance);
            return;
        }

        instance.SetActive(false);
        inactiveStacks[index].Push(instance);
    }
}
