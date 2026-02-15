/// 플레이어에 접근할때 필요한 요소들을 찾아서 세팅해주는 코드이다.
/// 실제 컴포넌트를 찾아서 PlayerContext 객체를 만든다.

using UnityEngine;

public class PlayerAgents : MonoBehaviour
{   
    public static PlayerAgents Instance { get; private set; } // 전역 인스턴스
    public PlayerContext Context { get; private set; }

    void Awake()
    {   
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        var status = GetComponent<PlayerStatus>();
        var inv = GetComponent<Inventory>();
        var wm = GetComponent<WeaponManager>();
        var currency = GetComponent<PlayerCurrency>();

        Context = new PlayerContext(status, inv, wm, currency);
    }
}
