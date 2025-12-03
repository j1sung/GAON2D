using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorPortal : MonoBehaviour
{
    [SerializeField] private GameObject sector;
    [SerializeField] private Transform t;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = t.position;
            sector.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
