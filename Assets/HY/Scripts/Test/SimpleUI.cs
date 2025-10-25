using UnityEngine;

public class SimpleUI: MonoBehaviour
{
    [SerializeField] private SelectionUIController selection;
    public KeyCode openKey = KeyCode.Q;
    public KeyCode closeKey = KeyCode.Escape;
    void Update()
    {
        if (Input.GetKeyDown(openKey)) selection.Open();
        if (Input.GetKeyDown(closeKey)) selection.Close();
    }
}
