using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRespawner : MonoBehaviour
{
    public Transform targetObject; // The target object to respawn
    public Transform respawnLocation; // The location to respawn the target object
    public LayerMask playerLayer; // Layer to detect collision with

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            RespawnTarget();
        }
    }

    private void RespawnTarget()
    {
        targetObject.position = respawnLocation.position;

        // Optionally reset velocity if the target object has a Rigidbody
        var rb = targetObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Target object respawned");
    }
}
