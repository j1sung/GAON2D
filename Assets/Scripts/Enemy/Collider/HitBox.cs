using UnityEngine;

public sealed class HitBox : MonoBehaviour
{
    [SerializeField] private Collider2D col;

    private float _damage;
    private bool _active;

    private void Reset() => col = GetComponent<Collider2D>();

    public void Arm(float damage)
    {
        _damage = damage;
        _active = damage > 0f;
        if (col) col.enabled = _active;
    }

    public void Disarm()
    {
        _active = false;
        _damage = 0f;
        if (col) col.enabled = _active;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_active) return;

        HurtBox hurtbox = other.GetComponent<HurtBox>();
        if (hurtbox == null) return;

        IDamageable dmg = hurtbox.Damageable;
        if (dmg == null) return;

        HitContext hit = new HitContext
        {
            damage = _damage,
        };
        dmg.ApplyHit(hit);
    }
}
