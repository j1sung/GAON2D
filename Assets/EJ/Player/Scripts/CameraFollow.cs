using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;

    void Start()
    {
        // 초기 위치에서 타겟과의 거리 자동 저장
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 자동 계산된 오프셋으로 위치 설정
        transform.position = target.position + offset;
    }
}