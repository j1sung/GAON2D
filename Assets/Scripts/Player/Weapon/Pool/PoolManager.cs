using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 문자열 키(poolName)로만 관리하는 초간단 풀 매니저.
/// - Get(): 스택에서 꺼내거나 Instantiate
/// - Release(): 비활성화 후 스택에 반납
/// - PoolObject.OnDespawn에 핸들러 자동 연결
/// </summary>
namespace Attack.Pooling
{
    public sealed class PoolManager : MonoBehaviour, IPool
{
    public static PoolManager instance;

    // 풀 스택: poolName → 비활성 객체 스택
    readonly Dictionary<string, Stack<GameObject>> pools = new Dictionary<string, Stack<GameObject>>();
    // 역매핑: 인스턴스 → poolName (반환 시 어떤 스택으로 갈지)
    readonly Dictionary<GameObject, string> reverse = new Dictionary<GameObject, string>();

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 프리팹 기반 인스턴스 획득:
    /// - PoolObject.poolName을 키로 사용 (없으면 prefab.name 사용)
    /// - 위치/회전/부모 설정 후 활성화
    /// </summary>
    public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        var key = ResolveKey(prefab);

        if (!pools.TryGetValue(key, out var stack))
        {
            stack = new Stack<GameObject>();
            pools[key] = stack;
        }

        GameObject go = (stack.Count > 0) ? stack.Pop() : Create(prefab, key);

        reverse[go] = key;

        if (parent != null) go.transform.SetParent(parent, false);
        go.transform.SetPositionAndRotation(pos, rot);

        go.SetActive(true);
        return go;
    }

    /// <summary>
    /// 사용 완료한 인스턴스 반환:
    /// - 비활성화 + PoolManager 자식으로 귀속 + 해당 스택에 push
    /// - 역매핑이 없으면 Destroy(풀 관리 대상 아님)
    /// </summary>
    public void Release(GameObject instance)
    {
        if (!reverse.TryGetValue(instance, out var key))
        {
            Destroy(instance);
            return;
        }

        instance.SetActive(false);
        instance.transform.SetParent(transform, false);
        pools[key].Push(instance);
    }

    // ---------- 내부 유틸 ----------

    // 프리팹에서 풀 키를 해석: PoolObject.poolName 우선, 없으면 prefab.name
    string ResolveKey(GameObject prefab)
    {
        var po = prefab.GetComponent<PoolObject>();
        if (po != null && !string.IsNullOrEmpty(po.poolName))
            return po.poolName;
        return prefab.name;
    }

    // 새 인스턴스 생성 + PoolObject 보장 + OnDespawn 핸들러 연결
    GameObject Create(GameObject prefab, string key)
    {
        var go = Instantiate(prefab, transform);
        var po = go.GetComponent<PoolObject>();
        if (po == null) po = go.AddComponent<PoolObject>();
        if (string.IsNullOrEmpty(po.poolName)) po.poolName = key;

        // Despawn() → Release() 연결
        po.OnDespawn -= HandleDespawn; // 중복 방지
        po.OnDespawn += HandleDespawn;

        return go;
    }

    // PoolObject의 Despawn 콜백 핸들러
    void HandleDespawn(PoolObject po)
    {
        if (po != null) Release(po.gameObject);
    }
}
}
