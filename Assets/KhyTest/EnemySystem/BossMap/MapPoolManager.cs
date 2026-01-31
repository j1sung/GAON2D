using System.Collections.Generic;
using UnityEngine;

public class MapPoolManager : MonoBehaviour
{
    public static MapPoolManager instance;

    private Dictionary<string, Stack<GameObject>> pools = new();
    private Dictionary<GameObject, string> reverse = new();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject Get(string poolName, GameObject prefab = null)
    {
        if (!pools.TryGetValue(poolName, out var stack))
        {
            stack = new Stack<GameObject>();
            pools.Add(poolName, stack);
        }

        GameObject go;

        if (stack.Count > 0)
        {
            go = stack.Pop();
        }
        else
        {
            if (prefab == null)
            {
                Debug.LogError($"[PoolManager] No prefab for pool '{poolName}'");
                return null;
            }

            go = Instantiate(prefab);
        }

        reverse[go] = poolName;
        go.SetActive(true);
        return go;
    }

    public void Release(GameObject go)
    {
        if (!reverse.TryGetValue(go, out var poolName))
        {
            Debug.LogWarning("[PoolManager] Trying to release unknown object");
            go.SetActive(false);
            return;
        }

        go.SetActive(false);
        pools[poolName].Push(go);
    }
}
