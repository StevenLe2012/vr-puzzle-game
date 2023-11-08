using System;
using System.Collections;
using System.Collections.Generic;
using Foundry;
using Foundry.Networking;
using UnityEngine;

public enum PlayerRole
{
    Player1,
    Player2
}
public class UpdateLayersOnGrab : NetworkComponent
{
    [SerializeField] private PlayerRole playerRole;
    
    private NetworkProperty<int> _layerIndex = new(0);
    
    private SpatialGrabbable _spatialGrabbable;

    public int LayerIndex
    {
        get => _layerIndex.Value;
        set => _layerIndex.Value = value;
    }

    private void Start()
    {
        LayerIndex = LayerMask.NameToLayer("FoundryGrabbable");
    }

    /* RegisterProperties is called once when the component is added to the networked object on Awake,
     * this is where we connect up all our properties.*/
    public override void RegisterProperties(List<INetworkProperty> props)
    {
        // This callback is called both when the value is set locally and when it is set remotely.
        _layerIndex.OnValueChanged += layerIndex=>
        {
            print($"value of {gameObject.name} was changed to: {layerIndex}");
            // if (layerIndex == LayerMask.NameToLayer("FoundryPlayer1"))
            // {
            //     gameObject.layer = LayerMask.NameToLayer("FoundryGrabbable1");
            // }
            // else if (layerIndex == LayerMask.NameToLayer("FoundryPlayer2"))
            // {
            //     gameObject.layer = LayerMask.NameToLayer("FoundryGrabbable2");
            // }
            // else if (layerIndex == LayerMask.NameToLayer("FoundryGrabbable"))
            // {
            //     gameObject.layer = LayerMask.NameToLayer("FoundryGrabbable");
            // }
            gameObject.layer = layerIndex;
            
        };
        props.Add(_layerIndex);
    }


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
        if (IsOwner)
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
                // gameObject.layer = LayerMask.NameToLayer("FoundryGrabbable1");
                print("LayerIndex Before Player 1: " + LayerIndex);
                LayerIndex = LayerMask.NameToLayer("FoundryGrabbable1");
                gameObject.layer = LayerIndex;
                print("LayerIndex After Player 1: " + LayerIndex);
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
                // gameObject.layer = LayerMask.NameToLayer("FoundryGrabbable2");
                print("LayerIndex Before Player 2: " + LayerIndex);
                LayerIndex = LayerMask.NameToLayer("FoundryGrabbable2");
                gameObject.layer = LayerIndex;
                print("LayerIndex After Player 2: " + LayerIndex);
            }
        }
        
    }

    public void UpdateLayerAfterGrab(SpatialHand arg0, SpatialGrabbable arg1)
    {
        print("Returned grab layer");
        // gameObject.layer = LayerMask.NameToLayer("FoundryGrabbable");
        print("LayerIndex Before Return: " + LayerIndex);
        LayerIndex = LayerMask.NameToLayer("FoundryGrabbable");
        gameObject.layer = LayerIndex;
        print("LayerIndex After Return: " + LayerIndex);
    }
    
    
}
