using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // ������� �����ϴ� ����
    public GameObject[] prefabs;

    public int maxPoolSizes = 25; // �� ���� Ǯ�� �ִ� ���� -> ���� ���͸��� ���� �ʿ��� ����?

    // Ǯ ��� ����Ʈ��
    List<GameObject>[] pools;
    Transform[] poolGroups; // PoolManager ���� �׷� ������Ʈ �����

    private void Awake()
    {
        // ��� ������Ʈ Ǯ ����Ʈ �ʱ�ȭ
        pools = new List<GameObject>[prefabs.Length];
        poolGroups = new Transform[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();

            // ������ �̸����� �׷� ������Ʈ ����
            GameObject group = new GameObject(prefabs[i].name + "Pool");
            group.transform.parent = transform;
            poolGroups[i] = group.transform;
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ������ Ǯ�� ��Ȱ��ȭ �� ���� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {
            // �߰��ϸ� select ������ �Ҵ�
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // �� ã�Ҵٸ�?
        if (!select && pools[index].Count < maxPoolSizes)
        {
            // ���Ӱ� �����ϰ� select ������ �Ҵ�(�ش� ������ �׷� ������Ʈ ������ ����)
            select = Instantiate(prefabs[index], poolGroups[index]);
            pools[index].Add(select);
        }

        return select;
    }
}
