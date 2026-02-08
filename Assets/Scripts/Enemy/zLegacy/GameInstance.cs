//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameInstance : MonoBehaviour
//{
//    public static GameInstance Instance;
//    public PlayerController player;
//    public EnemyPoolManager pool;

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        Instance = this;
//        DontDestroyOnLoad(gameObject);

//        SceneManager.sceneLoaded += OnSceneLoaded;
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        if (player == null)
//        {
//            FindPlayer();
//        }
//    }

//    private void FindPlayer()
//    {
//        player = FindObjectOfType<PlayerController>();
//    }

//    private void OnDestroy()
//    {
//        SceneManager.sceneLoaded -= OnSceneLoaded;
//    }
//}
