using UnityEngine;

public class UIPanelToggle : MonoBehaviour
{
    [SerializeField] private GameObject targetPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (targetPanel != null)
                targetPanel.SetActive(false);
        }
    }
}