using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    // This method is called when the GameObject enters a trigger collider.
    private bool hasBeenTriggered = false;

    public float moveDistance = 0.2f;
    public LayerMask triggerLayer;
    public bool buttonTriggered;


    private void OnTriggerEnter(Collider other)
    {
        //Check if the entering object is on the "TriggeringLayer"
        if (!hasBeenTriggered && ((1 << other.gameObject.layer) & triggerLayer) != 0)
        {
            hasBeenTriggered = true; // Set the flag to true to indicate that it has been triggered
        }
        print("triggered");

        GetComponent<Collider>().enabled = false;
        transform.position += Vector3.down * moveDistance;

        // players need to have BOTH buttons pressed to trigger the sliding door
        buttonTriggered = true;
    }
}
