using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour
{
    public GameObject protal; // 섹터 포탈
    public Transform rewardSpot; // 보상 소환 지점

    private bool first;

    private void OnEnable()
    {
        first = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && first)
        {
            SectorController.Instance.SetCurrentSector(this);
            first = false;
        }
    }
}
