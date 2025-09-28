using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public void Load(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }
    private IEnumerator LoadAsync(string name)
    {
        var op = SceneManager.LoadSceneAsync(name);
        while (!op.isDone) yield return null;
    }
}