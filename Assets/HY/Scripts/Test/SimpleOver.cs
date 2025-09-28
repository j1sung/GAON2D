using UnityEngine;
using TMPro;

public class SimpleOver : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI label;
    void OnEnable() { GameEvents.OnGameOver += Show; }
    void OnDisable() { GameEvents.OnGameOver -= Show; }
    void Show()
    {
        if (panel) panel.SetActive(true);
        if (label) label.text = "TIME UP!";
    }
}
