using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/StageFlowData", fileName = "StageFlowData")]

public class StageFlowData : ScriptableObject
{
    [System.Serializable]
    public class StageInfo
    {
        public string stageSceneName;
        public string bossSceneName;
        public AudioClip bgm;
        public Sprite thumbnail;
        public string rewardItem;
    }

    [Header("Scene Flow Settings")]
    public string lobbyScene = "Lobby";
    public AudioClip bgm;
    public List<StageInfo> stages = new();

}