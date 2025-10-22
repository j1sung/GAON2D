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
        anim.Play(0, 0, 0f);      // 0프레임부터 재생
        CancelInvoke();
        Invoke(nameof(DespawnSelf), len);
    }

    void OnDisable() => CancelInvoke();

    void DespawnSelf() => po.Despawn();
}