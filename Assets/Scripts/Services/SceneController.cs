using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Load(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            return;
        }

        if (Instance == null)
        {
            return;
        }

        StartCoroutine(LoadAsync(sceneName));
    }

    private IEnumerator LoadAsync(string name)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(name);
        while (!op.isDone)
            yield return null;

        GameEvents.OnSceneLoaded?.Invoke();
    }
}
