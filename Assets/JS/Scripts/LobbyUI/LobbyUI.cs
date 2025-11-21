using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class LobbyUI : MonoBehaviour
{
    public PlayerController pc;
    public GameObject tutorialUI;

    public TMP_Text text;
    public AudioSource audioSource;
    public AudioClip textSound;

    [TextArea] public string[] description;

    private Coroutine tutorialRoutine;

    private bool isReady; // 설명끝나면

    void Start()
    {
        pc.enabled = false;
        tutorialRoutine = StartCoroutine(Tutorial());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isReady)
        {
            pc.enabled = true;
            isReady = false;
            tutorialUI.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (tutorialRoutine != null)
            {
                StopCoroutine(tutorialRoutine);
                tutorialRoutine = null;
            }

            if (!pc)
            {
                pc = FindObjectOfType<PlayerController>();
            }
            pc.enabled = true;
            isReady = false;
            tutorialUI.SetActive(false);
        }
    }

    public IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < description.Length; i++)
        {
            PlaySound(textSound);
            text.text = description[i];
            TMPDOText(text, 1f);
            yield return new WaitForSeconds(3.5f);
        }

        if (!pc)
        {
            pc = FindObjectOfType<PlayerController>();
        }
        isReady = true;
    }

    public void TMPDOText(TMP_Text text, float duration)
    {
        text.maxVisibleCharacters = 0;
        DOTween.To(x=> text.maxVisibleCharacters = (int)x, 0f, text.text.Length, duration);
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
