using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public LayerMask playerLayer; // Set the Player layer in the Unity Inspector.

    public GameObject teleportDestination; // Drag and drop the TeleportDestination GameObject here in the Unity Inspector.

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            other.transform.position = teleportDestination.transform.position;
        }
    }
}
