using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource bgmSource;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
    }

    public void PlayBGM(AudioClip clip, float fadeTime = 0.5f)
    {
        if (clip == null) return;

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        StartCoroutine(FadeInBGM(clip, fadeTime));
    }

    public void StopBGM(float fadeTime = 0.5f)
    {
        StartCoroutine(FadeOutBGM(fadeTime));
    }

    private IEnumerator FadeInBGM(AudioClip newClip, float duration)
    {
        if (duration > 0f)
        {
            yield return FadeOutBGM(duration * 0.5f);
        }

        bgmSource.clip = newClip;
        bgmSource.Play();

        float t = 0;
        bgmSource.volume = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
    }

    private IEnumerator FadeOutBGM(float duration)
    {
        float startVol = bgmSource.volume;

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVol, 0, t / duration);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.volume = 1f;
    }
}
