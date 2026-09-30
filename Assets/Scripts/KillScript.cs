using System;
using Unity.VisualScripting;
using UnityEngine;

public class KillScript : MonoBehaviour
{

    private Vector2 respawnLocation = new Vector2(0f, 0f);
    private float respawnSmokeTime;
    private float respawnSmokeCount = 0.5f;
    [SerializeField] private Rigidbody2D rigid2D;
    [SerializeField] private GameObject deathSmoke; 

    // Update is called once per frame
    void Update()
    {
        if (rigid2D.position.y < -6f)
        {
            SpawnObject();
            rigid2D.position = respawnLocation;
            SpawnObject();
        }
    }


    void SpawnObject()
    {
        respawnSmokeTime = respawnSmokeCount;
        Vector2 spawnPointSmoke = rigid2D.position;

        // 1. Store the instantiated smoke clone in a temporary variable
        GameObject smokeClone = Instantiate(deathSmoke, spawnPointSmoke, Quaternion.identity);

        // 2. Automatically destroy the clone after a set time (e.g., 2 seconds)
        // Replace 2.0f with however long your smoke animation lasts.
        Destroy(smokeClone, 1.0f); 
    }

}
