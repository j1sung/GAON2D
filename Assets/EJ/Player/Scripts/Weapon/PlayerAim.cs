using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
public class PlayerAim : MonoBehaviour
{
    [Header("Flip 대상(스프라이트/총/FirePoint를 전부 이 밑으로)")]
    [SerializeField] Transform graphics;   // 없으면 자기 자신
    [SerializeField] SpriteRenderer sprite;// 쓰지 않으면 비워둬도 됨(중복 Flip 방지)

    Camera cam;
    WeaponManager weapon;
    Transform self;
    Vector3 baseScale;  // graphics의 원래 스케일 보존

    void Awake()
    {
        weapon = GetComponent<WeaponManager>();
        self   = transform;
        cam    = Camera.main != null ? Camera.main : FindObjectOfType<Camera>();
        if (!graphics) graphics = transform;   // 지정 안 했으면 자기 자신
        baseScale = graphics.localScale;

        // 스프라이트 flipX는 사용 안 함(중복 반전 방지)
        if (sprite) sprite.flipX = false;
    }

    void Update()
    {
        if (!cam) return;

        // 마우스 월드좌표(z는 플레이어 평면으로 고정)
        Vector3 mp = Input.mousePosition;
        float zDist = Mathf.Abs(cam.transform.position.z - self.position.z);
        Vector3 mw = cam.ScreenToWorldPoint(new Vector3(mp.x, mp.y, zDist));
        mw.z = self.position.z;

        // 조준 벡터 전달 (무기는 회전하지 않아도 탄은 aimDir로 나감)
        Vector2 aim = (mw - self.position);
        if (aim.sqrMagnitude > 1e-6f) weapon.SetAimDir(aim.normalized);

        // 화면 기준 좌/우 판정
        float sign = (mw.x >= self.position.x) ? 1f : -1f;

        // 그래픽 루트만 좌우 반전(루트 물리/위치 불변)
        var s = baseScale;
        s.x = Mathf.Abs(baseScale.x) * sign;   // 누적오차 방지: 항상 base 기준
        graphics.localScale = s;

        // (선택) sprite.flipX는 쓰지 않음. 쓰면 여기서만 true/false 설정
        // if (sprite) sprite.flipX = (sign < 0f);
    }
}