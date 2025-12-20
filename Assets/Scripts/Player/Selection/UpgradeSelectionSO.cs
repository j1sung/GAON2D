using UnityEngine;

[CreateAssetMenu(menuName = "SO/UpgradeSelection")]
public class UpgradeSelectionSO : ScriptableObject
{   
    public enum UpgradeType // 업그레이드 유형
    {
        status, item 
    }
    public string selectionName; // 이름
    public Sprite selectionImage; // 이미지
    public string selectionDescription; // 설명

    public UpgradeEffectSO effect; // 효과
}
