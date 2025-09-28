using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/GameConfig")]
public class GameConfing : ScriptableObject {
    [Min(10f)] public float gameDurationSeconds = 600f;
    public string nextSceneName = "NextStage";
    public KeyCode interactKey = KeyCode.E;
}
