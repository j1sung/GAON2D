using UnityEngine;
using Cinemachine;
using System;

public sealed class CameraRecoil : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulse;
    [SerializeField] private PlayerStatus status; // Inspector로 할당하거나 Find

    [Header("Hit 반동")]
    [SerializeField] private float hitScale = 0.01f;   // dmg * hitScale

    private void OnEnable()
    {
        if (status != null) status.OnHit += OnHit;
    }

    private void OnDisable()
    {
        if (status != null) status.OnHit -= OnHit;
    }

    private void OnHit(float value)
    {
        float amp = value * hitScale;
        Hit(amp);
    }

    public void Fire(float amplitude, Vector2 aimDir)
    {
        if (impulse == null || amplitude <= 0f) return;

        if (aimDir.sqrMagnitude < 0.0001f) aimDir = Vector2.right;
        impulse.GenerateImpulse(-(Vector3)aimDir.normalized * amplitude);
    }

    public void Hit(float amplitude)
    {
        if (impulse == null || amplitude <= 0f) return;
        impulse.GenerateImpulse(amplitude);
    }
}