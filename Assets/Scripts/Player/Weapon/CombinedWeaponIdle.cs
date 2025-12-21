using UnityEngine;

/// 씬에 배치된 로컬 위치를 그대로 기준으로 사용.
/// 좌우 전환은 회전으로만 처리하고, 위치는 따라가기만 한다.
public class CombinedWeaponIdleFollow : MonoBehaviour
{
    [Header("필수")]
    public Transform followTarget;

    [Header("Follow")]
    public float followDamping = 12f;

    [Header("Idle Motion")]
    public float bobAmplitude = 0.08f;
    public float bobSpeed = 2.0f;
    public float phaseOffset = 0f;

    // 내부 상태
    Vector3 baseLocalPos;   // ⭐ 씬에 배치된 기준 위치
    float t;
    float facingSign = 1f;

    void Awake()
    {
        if (!followTarget)
            followTarget = transform.root;
    }

    void OnEnable()
    {
        if (!followTarget) return;

        // ⭐ 씬에서 배치한 현재 위치를 기준으로 저장
        baseLocalPos = followTarget.InverseTransformPoint(transform.position);

        // 초기 방향
        facingSign = baseLocalPos.x >= 0f ? 1f : -1f;
    }

    void LateUpdate()
    {
        if (!followTarget) return;

        t += Time.deltaTime;

        // ===== 현재 방향 (PlayerAim의 flip 결과) =====
        float sign = Mathf.Sign(transform.lossyScale.x);
        if (sign != 0f)
            facingSign = sign;

        // ===== 기준 위치를 좌우 회전 =====
        Quaternion rot = Quaternion.Euler(0f, facingSign > 0f ? 0f : 180f, 0f);
        Vector3 rotatedLocal = rot * baseLocalPos;

        // ===== 부유 (Y축만) =====
        float bob = Mathf.Sin(t * bobSpeed + phaseOffset) * bobAmplitude;

        Vector3 targetLocal = rotatedLocal + new Vector3(0f, bob, 0f);

        // ===== 월드 이동 =====
        Vector3 desiredWorld = followTarget.TransformPoint(targetLocal);
        float k = 1f - Mathf.Exp(-followDamping * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desiredWorld, k);
    }
}