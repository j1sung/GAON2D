using UnityEngine;

/// 플레이어를 따라가되, 최초 배치 시점의 "플레이어 기준 오프셋"을 유지
public class CombinedWeaponIdleFollow : MonoBehaviour
{
    [Header("필수")]
    public Transform followTarget;          // 보통 Player 루트 Transform

    [Header("이펙트")]
    public float bobAmplitude = 0.08f;      // 상하 진폭
    public float bobSpeed = 2.0f;           // 상하 속도
    public float swayAmplitude = 0.15f;     // 좌우 진폭
    public float followDamping = 12f;       // 플레이어를 따라붙는 속도(클수록 더 빠름)

    [Header("거리 고정 옵션")]
    public bool keepDistance = true;        // true: 최초 거리(반경) 유지
    public bool flipToAim = true;           // 마우스 좌/우에 따라 X축 반전(스프라이트만)

    // 내부 상태
    Vector3 initialLocal;                   // 활성화 당시의 "플레이어 기준 로컬 위치"
    float initialRadius;                    // 거리(반경)
    float t;

    void Awake()
    {
        if (!followTarget) followTarget = transform.root;
    }

    void OnEnable()
    {
        // 플레이어 기준 로컬 위치를 앵커로 저장
        if (followTarget)
        {
            initialLocal   = followTarget.InverseTransformPoint(transform.position);
            initialRadius  = new Vector2(initialLocal.x, initialLocal.y).magnitude;
        }
    }

    void LateUpdate()
    {
        if (!followTarget) return;

        t += Time.deltaTime;

        // 1) 기본 로컬 앵커 + (bob/sway) 흔들림
        float bob  = Mathf.Sin(t * bobSpeed) * bobAmplitude;
        float sway = Mathf.Sin(t * (bobSpeed * 0.6f)) * swayAmplitude;

        Vector3 targetLocal = initialLocal + new Vector3(sway, bob, 0f);

        // 2) 거리(반경) 유지 옵션
        if (keepDistance)
        {
            Vector2 v = new Vector2(targetLocal.x, targetLocal.y);
            float m = v.magnitude;
            if (m > 1e-4f)
            {
                // 벡터 길이를 최초 반경으로 정규화해서, 흔들려도 거리는 유지
                v = v * (initialRadius / m);
                targetLocal.x = v.x;
                targetLocal.y = v.y;
            }
        }

        // 3) 월드 좌표로 변환해서 부드럽게 이동
        Vector3 desiredWorld = followTarget.TransformPoint(targetLocal);
        float k = 1f - Mathf.Exp(-followDamping * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desiredWorld, k);

        // 4) 시각적 플립(원하면 끄기)
        if (flipToAim && Camera.main)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            float aimSign = (Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= followTarget.position.x) ? 1f : -1f;
            var ls = transform.localScale;
            ls.x = Mathf.Abs(ls.x) * aimSign;
            transform.localScale = ls;
#endif
        }
    }

    /// 필요 시, 현재 위치를 기준으로 다시 앵커를 재설정
    public void ReanchorToCurrent()
    {
        initialLocal  = followTarget ? followTarget.InverseTransformPoint(transform.position) : transform.localPosition;
        initialRadius = new Vector2(initialLocal.x, initialLocal.y).magnitude;
    }
}