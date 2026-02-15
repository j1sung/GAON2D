using UnityEngine;

public sealed class ShopNPC : MonoBehaviour
{
    [SerializeField] private ShopUIController shopUI;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool inRange;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        inRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        inRange = false;
    }

    void Update()
    {
        if (!shopUI) return;

        if (Input.GetKeyDown(interactKey) && inRange && !shopUI.IsOpen)
        {
            shopUI.Open();
        }
    }
}