/// Cinemachine 카메라에 반동을 주는 코드
/// CinemachineImpulseSource로부터 발생한 이벤트를 감지해서 aimrDir 방향으로 amplitude만큼 힘을 준다.

using UnityEngine;
using Cinemachine;

public sealed class CameraRecoil : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulse;

    public void Fire(float amplitude, Vector2 aimDir)
    {
        if (impulse == null || amplitude <= 0f) return;

        if (aimDir.sqrMagnitude < 0.0001f)
            aimDir = Vector2.right;

        impulse.GenerateImpulse(-(Vector3)aimDir.normalized * amplitude);
    }
}