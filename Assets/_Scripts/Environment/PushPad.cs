using System;
using System.Collections;
using Foundry;
using UnityEngine;
using UnityEngine.Events;

public class PushPad : MonoBehaviour
{
    public NetworkEvent<bool> OnPadPushedEvent;
    public NetworkEvent<bool> OnPadRetractedEvent;
    // public UnityEvent OnPadPushed;  // do networkedevents. this will syncronise the push pads pushing down as well!
    // public UnityEvent OnPadRetracted;

    [SerializeField] private float waitForSecondsTillPadTakesEffect = 1.5f;
    [SerializeField] private LayerMask triggerLayer;
    [SerializeField] private float moveDistance = 0.2f;
    
    private bool _isOnPad;
    private Coroutine _waitCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!_isOnPad && ((1 << other.gameObject.layer) & triggerLayer.value) != 0)  // Make sure it's the player that triggers the event
        {
            print("Correct Object Triggered");
            _isOnPad = true;
            transform.position += Vector3.down * moveDistance;
            _waitCoroutine = StartCoroutine(WaitOnPad());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == triggerLayer) // Make sure it's the player that triggers the event
        {
            _isOnPad = false;
            if (_waitCoroutine != null)
            {
                StopAllCoroutines();
                _waitCoroutine = null;
                transform.position += Vector3.up * moveDistance;
            }
            
            OnPadRetractedEvent?.Invoke(true);
        }
    }

    private IEnumerator WaitOnPad()
    {
        yield return new WaitForSeconds(waitForSecondsTillPadTakesEffect);
        if (_isOnPad)
        {
            CompleteAction();
        }
    }

    private void CompleteAction()
    {
        if (_isOnPad)
        {
            OnPadPushedEvent?.Invoke(true);
        }
    }
}
