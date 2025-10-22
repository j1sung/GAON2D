using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public static GameInstance Instance;
    public PlayerController player;
    public PoolManager pool;

    private void Awake()
    {
        Instance = this;
    }
}
