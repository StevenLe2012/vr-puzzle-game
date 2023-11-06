using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    // This method is called when the GameObject enters a trigger collider.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object has a specific tag (optional).
        if (other.gameObject.layer == LayerMask.NameToLayer("FoundryGrabbable"))
        {
            // Destroy the object this script is attached to.
            Destroy(gameObject);
        }
    }
}
