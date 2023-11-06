using System;
using System.Collections;
using System.Collections.Generic;
using Foundry;
using UnityEngine;

public enum PlayerRole
{
    Player1,
    Player2
}
public class UpdateLayersOnGrab : MonoBehaviour
{
    [SerializeField] private PlayerRole playerRole;
    
    private SpatialGrabbable _spatialGrabbable;

    private void Awake()
    {
        _spatialGrabbable = GetComponent<SpatialGrabbable>();
    }
    
    private void OnEnable()
    {
        _spatialGrabbable.OnBeforeGrabbedEvent.AddListener(UpdateLayerBeforeGrab);
        _spatialGrabbable.OnReleaseEvent.AddListener(UpdateLayerAfterGrab);
    }
    
    private void OnDisable()
    {
        _spatialGrabbable.OnBeforeGrabbedEvent.RemoveListener(UpdateLayerBeforeGrab);
        _spatialGrabbable.OnReleaseEvent.RemoveListener(UpdateLayerAfterGrab);
    }

    public void UpdateLayerBeforeGrab(SpatialHand spatialhand, SpatialGrabbable spatialGrabbable)
    {
        
        if (playerRole == PlayerRole.Player1)
        {
            if (spatialhand.gameObject.layer != LayerMask.NameToLayer("FoundryPlayer1"))
            {
                print("Picked up by invalid player");
                spatialhand.CancelGrab();
                return;
            }
            
            print("Grabbed by player 1");
            gameObject.layer = LayerMask.NameToLayer("FoundryGrabbable1");
        }
        else
        {
            if (spatialhand.gameObject.layer != LayerMask.NameToLayer("FoundryPlayer2"))
            {
                print("Picked up by invalid player");
                spatialhand.CancelGrab();
                return;
            }
            
            print("Grabbed by player 2");
            gameObject.layer = LayerMask.NameToLayer("FoundryGrabbable2");
        }
    }

    public void UpdateLayerAfterGrab(SpatialHand arg0, SpatialGrabbable arg1)
    {
        print("Returned grab layer");
        gameObject.layer = LayerMask.NameToLayer("FoundryGrabbable");
    }
    
    
}
