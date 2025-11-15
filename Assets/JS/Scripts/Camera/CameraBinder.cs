using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBinder : MonoBehaviour
{
    void OnEnable()
    {

        var vcam = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineConfiner2D>();
        var sector = GetComponent<PolygonCollider2D>();

        if (vcam != null)
        {
            vcam.m_BoundingShape2D = sector;
        }
        else
            Debug.LogWarning("카메라 참조 실패!");
    }
}
