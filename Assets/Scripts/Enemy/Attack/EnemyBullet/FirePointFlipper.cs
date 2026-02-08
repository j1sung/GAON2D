using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointFlipper : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    Vector3 _defaultLocal; // 로컬 위치 저장

    private void Awake()
    {
        // 로컬 위치 초기화
        _defaultLocal = transform.localPosition;
    }

    private void LateUpdate()
    {
        bool faceLeft = sprite.flipX;
        Vector3 p = _defaultLocal;

        // 현재 스프라이트 flipX를 보고 firePoint 로컬 위치도 변환
        p.x = faceLeft ? -Mathf.Abs(p.x) : Mathf.Abs(p.x);

        // firePoint 회전 적용
        transform.localPosition = p;
    }
}
