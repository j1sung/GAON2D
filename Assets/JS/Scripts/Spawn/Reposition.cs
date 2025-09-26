using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area"))
        {
            Debug.Log("omg!!");
            return;
        }

        switch (transform.tag)
        {
            case "Enemy":
                if (coll.enabled) // ���� ���� �ݶ��̴� ����
                {
                    // �÷��̾��� ���� ��ġ�� �����ɴϴ�.
                    Vector2 playerPos = GameInstance.Instance.player.transform.position;
                    Vector2 playerDir = GameInstance.Instance.player.Movement;
                    //float dirX = playerDir.x < 0 ? -1 : 1;
                    //float dirY = playerDir.y < 0 ? -1 : 1;

                    // �÷��̾� ��ġ�� �������� ��ǥ�� ���
                    float distance = 13f;
                    Vector2 randomOffset = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));

                    //position�� ���� �� ��ġ�� �Ҵ��մϴ�.
                    transform.position = playerPos + playerDir * distance + randomOffset; 

                    // ���� ������Ʈ Ǯ�� ȸ�� & ���� �ʱ�ȭ ���� ����
                }
                break;
        }
    }
}
