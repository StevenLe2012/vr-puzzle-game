using System;
using System.Collections;
using System.Collections.Generic;
using Foundry;
using Foundry.Networking;
using UnityEngine;

public class NewSceneNetworkedEvent : NetworkComponent
{
    public NetworkEvent<bool> NewSceneEvent;

    // private void OnTriggerEnter(Collider other)
    // {
    //     NewSceneEvent?.Invoke(true);
    //     // Invoke does everyone
    //     // Invoke Remote does everyone except the local person triggering
    //     // Invoke Local does only the local person triggering
    // }
    
    
}
