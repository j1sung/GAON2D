using UnityEngine;

[CreateAssetMenu(menuName = "SO/Core")]
public class CoreSO : ScriptableObject
{   
    public enum CoreType { main, sub };

    [Header("영구 코드")]
    public string code;

    [Header("코어 데이터")]
    public CoreType type;
    public string coreName;
    public Sprite image;
}
