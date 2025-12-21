using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(WeaponManager))]
[RequireComponent(typeof(PlayerStatus))]
public class PlayerAim : MonoBehaviour
{
    [Header("Flip 대상")]
    [SerializeField] Transform graphics;

    private Camera cam;
    private WeaponManager weapon;
    private PlayerController status;   // ✅ 추가
    private Transform self;
    private Vector3 baseScale;

    private float facingSign = 1f;

    void Awake()
    {
        weapon = GetComponent<WeaponManager>();
        status = GetComponent<PlayerController>();  // ✅ 추가
        self   = transform;
        cam    = Camera.main != null ? Camera.main : FindObjectOfType<Camera>();

        if (!graphics) graphics = transform;
        baseScale = graphics.localScale;
    }

    void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam = Camera.main != null ? Camera.main : FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (!cam) return;

        // ✅ 죽으면 조준/flip 모두 정지
        if (status != null && status.isDead) return; // IsDead 없으면 status 기반으로 bool 하나 노출

        Vector3 mp = Input.mousePosition;
        float zDist = Mathf.Abs(cam.transform.position.z - self.position.z);
        Vector3 mw = cam.ScreenToWorldPoint(new Vector3(mp.x, mp.y, zDist));
        mw.z = self.position.z;

        if (Input.GetMouseButton(0))
            weapon.SetDefaultAimDir(mw);

        if (Input.GetMouseButton(1) || weapon.IsCombinedFiring)
            weapon.SetCombinedAimDir(mw);

        float deltaX = mw.x - self.position.x;
        if (Mathf.Abs(deltaX) > 0.01f)
            facingSign = deltaX > 0 ? 1f : -1f;

        var s = baseScale;
        s.x = Mathf.Abs(baseScale.x) * facingSign;
        graphics.localScale = s;
    }
}