using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(WeaponManager))]
public class PlayerAim : MonoBehaviour
{
    [Header("Flip 대상")]
    [SerializeField] Transform graphics;

    private Camera cam;
    private WeaponManager weapon;
    private Transform self;
    private Vector3 baseScale;  // graphics의 원래 스케일 보존

    void Awake()
    {
        weapon = GetComponent<WeaponManager>();
        self   = transform;
        cam    = Camera.main != null ? Camera.main : FindObjectOfType<Camera>();
        if (!graphics) graphics = transform;   // 지정 안 했으면 자기 자신
        baseScale = graphics.localScale;

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam = Camera.main != null ? Camera.main : FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (!cam) return;

        Vector3 mp = Input.mousePosition;
        float zDist = Mathf.Abs(cam.transform.position.z - self.position.z);
        Vector3 mw = cam.ScreenToWorldPoint(new Vector3(mp.x, mp.y, zDist));
        mw.z = self.position.z;

        // 조준 계산
        Vector2 aim = (mw - self.position);
        if (aim.sqrMagnitude > 1e-6f)
            weapon.SetAimDir(aim.normalized);

        // Flip 기준을 무조건 aimDir로 제한
        float sign = (weapon.aimDir.x >= 0f) ? 1f : -1f;

        var s = baseScale;
        s.x = Mathf.Abs(baseScale.x) * sign;
        graphics.localScale = s;
        Debug.Log($"Player rot = {graphics.rotation.eulerAngles}");
    }
}