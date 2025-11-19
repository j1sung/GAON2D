using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class VCamBinder : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;

    void Awake()
    {
        if (vcam == null)
        vcam = GetComponent<CinemachineVirtualCamera>();

        BindPlayer();
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
        BindPlayer();
    }

    private void BindPlayer()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        vcam.Follow = player.transform;
    }
}
