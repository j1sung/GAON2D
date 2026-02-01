using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    // 프리펩들 저장하는 변수
    public GameObject[] prefabs;

    public int maxPoolSizes = 10; // 각 몬스터 풀의 최대 개수 -> 추후 몬스터마다 설정 필요할 수도?

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
        poolGroups = new Transform[n];

        for (int i = 0; i < n; i++)
        {
            // 스택 배열 인덱스마다 적 종류별로 최대 풀 갯수 공간 만들기.
            inactiveStacks[i] = new Stack<GameObject>(maxPoolSizes);

            // 적 프리펩 이름으로 그룹 오브젝트 생성.
            GameObject group = new GameObject(prefabs[i].name + "Pool");

            // 생성한 그룹 오브젝트를 Pooling_Enemy 하위로 계층 이동.
            group.transform.SetParent(transform, false);

            // 그룹 배열에 만든 그룹 오브젝트 인덱스마다 대입
            poolGroups[i] = group.transform;
        }
    }

    // 적 생성 & 활성화 API
    public GameObject Get(int index)
    {
        // 잘못된 인덱스는 return.
        if(index < 0 || index >= prefabs.Length)
        {
            Debug.Log($"[EnemyPoolManager] 잘못된 index: {index}");
            return null;
        }

        // 반환값 초기화.
        GameObject select = null;

        // 선택한 적 풀의 비활성화 된 게임 오브젝트 꺼내기 (O(1))
        if(inactiveStacks[index].Count > 0)
        {
            select = inactiveStacks[index].Pop();
        }
        else
        {
            select = Instantiate(prefabs[index], poolGroups[index]);
            reverse[select] = index;
        }
        select.SetActive(true);

        // enemy에 EnemyPoolManager 주입.
        if(select.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.SetOwnerPool(this);
            enemy.ResetEnemy(); // 여기서 매번 풀링 소환될때 스테이터스를 초기화해줌 -> 추후 덮어씌우는 문제 생기면 변경 필요!
        }
            

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
