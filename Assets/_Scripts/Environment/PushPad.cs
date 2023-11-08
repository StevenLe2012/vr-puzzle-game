using System;
using System.Collections;
using System.Collections.Generic;
using Foundry;
using Foundry.Networking;
using UnityEngine;
using UnityEngine.Events;

public class PushPad : NetworkComponent
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
    
    /* RegisterProperties is called once when the component is added to the networked object on Awake,
     * this is where we connect up all our properties.*/
    public override void RegisterProperties(List<INetworkProperty> props)
    {
        // OnPadPushedEvent.AddListener(GetComponent<ChangeScene>().ChangeToSelectScene);
        props.Add(OnPadPushedEvent);   
        props.Add(OnPadRetractedEvent);   
    }

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
        if (_isOnPad && ((1 << other.gameObject.layer) & triggerLayer.value) != 0) // Make sure it's the player that triggers the event
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
