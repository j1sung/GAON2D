using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;

    [Header("SFX Pool")]
    [SerializeField] private AudioSource sfxPrefab;
    [SerializeField] private int sfxPoolSize = 10;

    private Queue<AudioSource> sfxPool = new();
    private Coroutine bgmFadeCoroutine;

    #region Unity LifeCycle

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitBGM();
        InitSFXPool();
    }

    #endregion

    #region Init

    private void InitBGM()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = 1f;
    }

    private void InitSFXPool()
    {
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource src = Instantiate(sfxPrefab, transform);
            src.playOnAwake = false;
            src.loop = false;
            src.gameObject.SetActive(false);
            sfxPool.Enqueue(src);
        }
    }

    #endregion

    #region BGM

    public void PlayBGM(AudioClip clip, float fadeTime = 0.5f)
    {
        if (clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        if (bgmFadeCoroutine != null)
            StopCoroutine(bgmFadeCoroutine);

        bgmFadeCoroutine = StartCoroutine(FadeInBGM(clip, fadeTime));
    }

    public void StopBGM(float fadeTime = 0.5f)
    {
        if (bgmFadeCoroutine != null)
            StopCoroutine(bgmFadeCoroutine);

        bgmFadeCoroutine = StartCoroutine(FadeOutBGM(fadeTime));
    }

    private IEnumerator FadeInBGM(AudioClip newClip, float duration)
    {
        if (bgmSource.isPlaying)
            yield return FadeOutBGM(duration * 0.5f);

        bgmSource.clip = newClip;
        bgmSource.volume = 0f;
        bgmSource.Play();

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        bgmSource.volume = 1f;
    }

    private IEnumerator FadeOutBGM(float duration)
    {
        float startVol = bgmSource.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(startVol, 0f, t / duration);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.volume = startVol;
    }

    #endregion

    #region SFX

    public void PlaySFX(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        AudioSource src = GetSFXSource();
        src.transform.position = position;
        src.clip = clip;
        src.gameObject.SetActive(true);
        src.Play();

        StartCoroutine(ReturnSFXSource(src));
    }

    public void PlaySFX(AudioClip clip)
    {
        PlaySFX(clip, Camera.main.transform.position);
    }

    private AudioSource GetSFXSource()
    {
        if (sfxPool.Count > 0)
            return sfxPool.Dequeue();

        // Ǯ ���� �� ������ġ (�ӽ� ����)
        AudioSource extra = Instantiate(sfxPrefab, transform);
        return extra;
    }

    private IEnumerator ReturnSFXSource(AudioSource src)
    {
        yield return new WaitWhile(() => src.isPlaying);

        src.clip = null;
        src.gameObject.SetActive(false);
        sfxPool.Enqueue(src);
    }

    #endregion

    #region Pause Control

    public void PauseAllAudio()
    {
        AudioListener.pause = true;
    }

    public void ResumeAllAudio()
    {
        AudioListener.pause = false;
    }

    #endregion
}
