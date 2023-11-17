using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpecificObject : MonoBehaviour
{
    public GameObject targetObject; // The specific object to push back
    private float teleportDistance = 1f; // The force to push back the target

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            Vector3 teleportDirection = (other.transform.position - transform.position).normalized;
            targetObject.transform.position += teleportDirection * teleportDistance;

            Debug.Log("Target object is teleported");
        }
    }
}
