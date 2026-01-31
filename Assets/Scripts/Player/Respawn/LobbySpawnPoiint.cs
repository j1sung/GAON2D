using UnityEngine;

public class LobbySpawnPoint : MonoBehaviour
{
    public static LobbySpawnPoint Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}