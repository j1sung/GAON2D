using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathDirection : MonoBehaviour
{
    private PlayerStatus _status;

    [Header("Death UI")]
    [SerializeField] private CanvasGroup _deathUI;
    [SerializeField] private float _fadeInDuration  = 0.5f;
    [SerializeField] private float _fadeOutDuration = 1.0f;

    void Awake()
    {
        _status = GetComponent<PlayerStatus>();
    }

    void OnEnable()
    {
        _status.OnDeath += DeathSequence;
    }

    void OnDisable()
    {
        _status.OnDeath -= DeathSequence;
    }

    public void BindDeathUI(CanvasGroup deathUI)
    {
        _deathUI = deathUI;
    }

    private void DeathSequence()
    {
        StartCoroutine(DeathFlow());
    }
    
    private IEnumerator DeathFlow()
    {   
        // 2초 대기
        yield return new WaitForSeconds(2f);

        // 페이드 인
        _deathUI.blocksRaycasts = true;
        yield return Fade(0f, 1f, _fadeInDuration);

        // 3초 대기
        yield return new WaitForSeconds(5f);

        // 죽음 연출 종료 이벤트 invoke
        GameEvents.OnDeathDirectionEnd?.Invoke();
        yield return Fade(1f, 0f, _fadeOutDuration);
  
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        _deathUI.alpha = from;

        while (t < duration)
        {
            t += Time.deltaTime;
            _deathUI.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        _deathUI.alpha = to;
    }
}
