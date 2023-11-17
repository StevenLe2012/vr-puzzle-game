using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAtLocation : MonoBehaviour
{
    public Transform respawnLocation; // The location to respawn the GameObject
    private const float ThresholdY = -10f; // The y-coordinate threshold

    private void Update()
    {
        if (transform.position.y < ThresholdY)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = respawnLocation.position;
        // Optionally reset velocity if the GameObject has a Rigidbody
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("GameObject respawned at target location");
    }
}
