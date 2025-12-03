/// 풀링 대상 오브젝트에 부착되는 공통 컴포넌트이다.
/// Despawn()을 호출하면 PoolManager로 복귀한다.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public string poolName;
    public Action<PoolObject> OnDespawn;

    // OnDespawn이 호출되면, 해당 객체를 Despawn한다.
    public void Despawn() => OnDespawn?.Invoke(this);
}
