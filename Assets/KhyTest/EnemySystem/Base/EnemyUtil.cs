using UnityEngine;

public static class EnemyUtil
{
    public static float Distance2D(Transform a, Transform b)
    {
        Vector2 pa = a.position;
        Vector2 pb = b.position;
        return Vector2.Distance(pa, pb);
    }

    public static Vector2 Direction2D(Transform from, Transform to)
    {
        Vector2 dir = (Vector2)(to.position - from.position);
        if (dir.sqrMagnitude < 0.0001f) return Vector2.zero;
        return dir.normalized;
    }
}
