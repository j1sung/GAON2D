using UnityEngine;

public sealed class HurtBox : MonoBehaviour
{
    [SerializeField] private MonoBehaviour owner; // IDamageable 구현체 드래그
    public IDamageable Damageable => owner as IDamageable;

    private void Awake()
    {
        if (Damageable == null)
            Debug.LogError($"{name}: owner must implement IDamageable", this);
    }
}
