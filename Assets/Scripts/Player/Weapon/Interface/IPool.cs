using UnityEngine;

public interface IPool
{
    // 프리팹 기반 인스턴스를 가져온다.
    GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent = null);
    // 사용을 마친 인스턴스를 풀로 반환
    void Release(GameObject instance);
}