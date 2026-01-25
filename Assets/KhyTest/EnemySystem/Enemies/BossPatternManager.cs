using UnityEngine;

public class BossPatternManager : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Patterns")]
    [SerializeField] private MonoBehaviour[] patternBehaviours;
    // 인스펙터에서 IBossPattern 구현 컴포넌트들 넣기

    private IBossPattern[] _patterns;

    private void Awake()
    {
        // MonoBehaviour 배열을 IBossPattern으로 캐스팅해서 사용
        _patterns = new IBossPattern[patternBehaviours.Length];

        for (int i = 0; i < patternBehaviours.Length; i++)
        {
            _patterns[i] = patternBehaviours[i] as IBossPattern;
            if (_patterns[i] == null)
            {
                Debug.LogError($"{name}: patternBehaviours[{i}] does not implement IBossPattern.", this);
            }
        }

        // target 자동 세팅 (원하면 EnemyState에서 넣어줘도 됨)
        if (target == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player) target = player.transform;
        }
    }

    public void ExecuteRandomPattern(EnemyBase boss, float attackPower)
    {
        if (_patterns == null || _patterns.Length == 0)
        {
            Debug.LogWarning($"{name}: No boss patterns assigned.", this);
            return;
        }

        int safety = 10; // null 패턴 섞였을 때 대비
        IBossPattern chosen = null;

        while (safety-- > 0 && chosen == null)
        {
            int idx = Random.Range(0, _patterns.Length);
            chosen = _patterns[idx];
        }

        if (chosen == null)
        {
            Debug.LogWarning($"{name}: All assigned patterns are invalid.", this);
            return;
        }

        chosen.Execute(boss, target, attackPower);
    }
}
