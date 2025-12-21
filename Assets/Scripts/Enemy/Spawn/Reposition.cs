using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    GameObject player;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area"))
            return;

        // Area �Ѿ�� ��ġ ������
        if (coll.enabled) // ����ִ� ���鸸 ��ġ ����, ���� ���� Ǯ�� ȸ�� -> ���� ���� �ݶ��̴� ��������
        {
            // �÷��̾��� ���� ��ġ�� �����ɴϴ�.
            Vector2 playerPos = player.transform.position;
            Vector2 playerDir = player.GetComponent<PlayerController>().movement;
            //Vector2 playerPos = GameInstance.Instance.player.transform.position;
            //Vector2 playerDir = GameInstance.Instance.player.Movement;
            //float dirX = playerDir.x < 0 ? -1 : 1;
            //float dirY = playerDir.y < 0 ? -1 : 1;

            // �÷��̾� ��ġ�� �������� ��ǥ�� ���
            float distance = 13f;
            Vector2 randomOffset = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));

            //position�� ���� �� ��ġ�� �Ҵ��մϴ�.
            transform.position = playerPos + playerDir * distance + randomOffset; 

            // ���� ������Ʈ Ǯ�� ȸ�� & ���� �ʱ�ȭ ���� ����
        }

    }
}
