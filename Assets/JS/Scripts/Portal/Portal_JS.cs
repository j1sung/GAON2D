using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_JS : MonoBehaviour
{
    [SerializeField] GameObject portal;
    [SerializeField] AudioSource audioSource;

    bool isFirst = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&isFirst)
        {
            portal.SetActive(true);
            audioSource.Play();
            isFirst = false;
        }
    }

}
