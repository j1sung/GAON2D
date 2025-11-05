using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class PlayerAgents : MonoBehaviour {
    public Inventory inventory { get; private set; }
    //public PlayerStatus status { get; private set; }
    // public LevelSystem level { get; private set; }

    void Awake() {
        inventory = GetComponent<Inventory>();
    //    status    = GetComponent<PlayerStatus>();
    //    level     = GetComponent<LevelSystem>();
    }

    public void GainExp(float amount) {
    //    if (status != null) status.GainExp(amount);
    //    else if (level != null) level.AddExp(amount);
    }
}