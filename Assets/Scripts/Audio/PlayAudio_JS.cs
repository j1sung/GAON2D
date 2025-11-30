using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio_JS : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    void Start()
    {
        audioSource.Play();
    }
}
