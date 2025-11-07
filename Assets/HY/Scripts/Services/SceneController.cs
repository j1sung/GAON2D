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
            Debug.LogWarning("[SceneController] 씬 이름이 비어 있습니다!");
            return;
        }

        if (Instance == null)
        {
            Debug.LogError("[SceneController] Instance가 null 상태에서 Load 호출됨!");
            return;
        }

        StartCoroutine(LoadAsync(sceneName));
    }

    private IEnumerator LoadAsync(string name)
    {
        Debug.Log($"[SceneController] Loading scene: {name}");

        AsyncOperation op = SceneManager.LoadSceneAsync(name);
        while (!op.isDone)
            yield return null;
    }
}
