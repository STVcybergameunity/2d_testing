using UnityEngine;

public class KillScript : MonoBehaviour
{
    private Vector2 respawnLocation = new Vector2(0f, 0f);
    [SerializeField] private Rigidbody2D rigid2D;
    [SerializeField] private GameObject deathSmoke;       // drag the prefab here
    private float smokeLifetime = 0.4f;

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (rigid2D.position.y < -6f)
        {
            SpawnObject();                    // smoke where you fell
            rigid2D.position = respawnLocation;
            SpawnObject();                    // smoke where you respawn
        }
    }

    void SpawnObject()
    {
        GameObject smokeClone = Instantiate(deathSmoke, rigid2D.position, Quaternion.identity);
        Destroy(smokeClone, smokeLifetime);
    }
}