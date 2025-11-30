using UnityEngine;

[CreateAssetMenu(menuName = "SO/Blueprint")]
public class BlueprintSO : ScriptableObject
{   
    [Header("영구 코드")]
    public string code;

    [Header("설계도 데이터")]
    public string blueprintName;
    public Sprite image;
}
