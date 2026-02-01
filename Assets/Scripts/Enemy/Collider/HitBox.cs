using UnityEngine;

public sealed class HitBox : MonoBehaviour
{
    [SerializeField] private Collider2D col;

    private float _damage; // 임시 데미지 저장소

    //private readonly Collider2D[] _results = new Collider2D[16];
    //private ContactFilter2D _filter;

    private void Awake()
    {
        if (!col) col = GetComponent<Collider2D>();

        //_filter = new ContactFilter2D
        //{
        //    useLayerMask = true,
        //    layerMask = LayerMask.GetMask("Player"),
        //    useTriggers = true
        //};
    }
    //private Collider2D _ignoreEnterFor; // 이번 Arm에서 Overlap로 처리한 대상(들) 중 하나

    public void BodyDamage(float damage) => _damage = damage;
    //public void Arm(float damage)
    //{
    //    _damage = damage; // 데미지 넣기

    //    _ignoreEnterFor = null;

    //    // 이미 겹쳐있는 상태면 "Arm 순간" 1회 히트
    //    int count = col.OverlapCollider(_filter, _results);
    //    for (int i = 0; i < count; i++)
    //    {
    //        Collider2D c = _results[i];
    //        if (!c) continue;

    //        // Overlap로 히트한 대상은 Enter 중복을 막기 위해 저장
    //        // (여러 개면 모두 저장하는게 정석이지만 "겹친 상태에서 토글"은 보통 1개라 최소로 1개만)
    //        _ignoreEnterFor ??= c;

    //        HitOnce(c);
    //    }
    //}

    // 겹쳐있었을 경우에 콜라이더 활성화시 1번 타격 -> 콜라이더 중 Player 레이어인 콜라이더만 타격!
    //private void HitOnce(Collider2D other)
    //{
    //    if (!other) return;
    //    if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

    //    var dmg = other.GetComponentInParent<IDamageable>();
    //    if (dmg == null) return;

    //    dmg.ApplyHit(new HitContext { damage = _damage });
    //}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_damage <= 0f)
        {
            Debug.Log("damage: " + _damage);
            return;
        }
   

        // Overlap로 이미 처리한 "그 콜라이더"의 Enter만 무시
        //if (_ignoreEnterFor != null && other == _ignoreEnterFor)
        //{
        //    _ignoreEnterFor = null;
        //    return;
        //}

        // 콜라이더 레이어 체크
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        IDamageable dmg = other.GetComponentInParent<IDamageable>();

        if (dmg == null) return;

        dmg.ApplyHit(new HitContext { damage = _damage });
    }
}
