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
    [SerializeField] float spawnTime; // �����Ǵ� �ð�

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        // ���� ��Ÿ���� ���� ������
        time += Time.deltaTime;
        if(time > spawnTime) // ������ �ٸ� ���� Ÿ�� ���� �ʿ�!!
        {
            time = 0f;
            Spawn();
        }
    }
    void Spawn() // ���� GameManager�� �ð�, �̺�Ʈ, ���ǿ� ���� �� �������� ����
    {
        spawnCount++;
        //spawnIndex = spawnCount % 3 == 0 ?  1 : 0;

        GameObject enemyObj = GameInstance.Instance.pool.Get(spawnIndex);
        if (enemyObj == null)
        {
            // �ִ�ġ ���޷� ���� ���� ����
            return;
        }

        enemyObj.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // ���� ��ȯ ��ġ ���� ����Ʈ ����... Range 0�� Spawner Ŭ���� ��ġ�� ��
    }
}
