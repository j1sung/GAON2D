using UnityEngine;

public class SelectionUIController : MonoBehaviour
{
    [SerializeField] private GameObject panel; 

    public void Open()
    {
        panel.SetActive(true);
        GameEvents.OnSelectionOpened?.Invoke();
    }
    public void Close()
    {
        panel.SetActive(false);
        GameEvents.OnSelectionClosed?.Invoke();
    }

    public void OnLevelUp() => Open();

    public void OnSelectOption() => Close();
}
