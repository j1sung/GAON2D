using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PoolObject))]
public class ImpactAutoDespawn : MonoBehaviour
{
    [SerializeField] AnimationClip clip; // 반드시 할당(길이로 타이머 계산)

    Animator anim;
    PoolObject po;
    float len;

    void Awake()
    {
        anim = GetComponent<Animator>();
        po   = GetComponent<PoolObject>();
        len  = (clip && clip.length > 0f) ? clip.length : 0.01f; // 최소 안전값
    }

    void OnValidate()
    {
        if (clip && clip.length > 0f) len = clip.length;
    }

    void OnEnable()
    {
        // 애니메이터 초기화
        var anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Rebind();          // Animator 상태 리셋
            anim.Update(0f);        // 즉시 갱신
            anim.Play(0, 0, 0f);    // 첫 프레임부터 재생
        }

        // 혹시 이전 Invoke 남아 있으면 취소하고 새로 시작
        CancelInvoke(nameof(DespawnSelf));
        Invoke(nameof(DespawnSelf), len);
    }

    void OnDisable() => CancelInvoke();

    void DespawnSelf() => po.Despawn();
}