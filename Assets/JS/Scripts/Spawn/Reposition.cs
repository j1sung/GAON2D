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
                if (coll.enabled) // 죽은 적은 콜라이더 꺼짐
                {
                    // 플레이어의 현재 위치를 가져옵니다.
                    Vector2 playerPos = GameInstance.Instance.player.transform.position;
                    Vector2 playerDir = GameInstance.Instance.player.Movement;
                    //float dirX = playerDir.x < 0 ? -1 : 1;
                    //float dirY = playerDir.y < 0 ? -1 : 1;

                    // 플레이어 위치를 기준으로 좌표를 계산
                    float distance = 13f;
                    Vector2 randomOffset = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));

                    //position에 직접 새 위치를 할당합니다.
                    transform.position = playerPos + playerDir * distance + randomOffset; 

                    // 추후 오브젝트 풀링 회수 & 스텟 초기화 구문 변경
                }
                break;
        }
    }
}
