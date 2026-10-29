using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField] private Transform hitBox;
    private KillScript kill;
    private bool isCollected = false;

    private void Start()
    {
        kill = FindFirstObjectByType<KillScript>();
    }

    private void Update()
    {
        if (!isCollected && kill != null && hitBox != null
            && Vector2.Distance(hitBox.position, transform.position) < 1.5f)
        {
            kill.SetRespawnLocation(transform.position);
            isCollected = true;
        }
    }
}
