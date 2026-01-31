using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorPortal : MonoBehaviour
{
    [SerializeField] private GameObject sector;
    [SerializeField] private Transform t;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 콜라이더가 닿았을 경우.
        if (collision.CompareTag("Player"))
        {
            // 플레이어 루트 오브젝트 위치 이동.
            collision.transform.root.position = t.position;
            
            // 해당 섹터 활성화.
            sector.SetActive(true);

            // 기존 섹터 비활성화.
            transform.parent.gameObject.SetActive(false);
        }
    }
}
