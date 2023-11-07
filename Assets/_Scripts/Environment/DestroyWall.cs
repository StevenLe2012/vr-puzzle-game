using System.Collections.Generic;
using Foundry.Networking;
using UnityEngine;

public class DestroyWall : MonoBehaviour // then i can change to monobehvaior.
{
    // private NetworkProperty<int> _buttonsHeldDownTillWallBreaks = new(0);  // after syncronizing event, i dont need to network this anymore. they both check locally if this number is greater than 2 than destroy wall.
    public int ButtonsHeldDownTillWallBreaks = 2;
    
    [HideInInspector]
    public int CurrentButtonsHeld = 0;

    /* RegisterProperties is called once when the component is added to the networked object on Awake,
     * this is where we connect up all our properties.*/
    // public override void RegisterProperties(List<INetworkProperty> props)
    // {
    //     // This callback is called both when the value is set locally and when it is set remotely.
    //     _buttonsHeldDownTillWallBreaks.OnValueChanged += buttonsHeldDown=>
    //     {
    //         ButtonsHeldDownTillWallBreaks = buttonsHeldDown;
    //     };
    //     props.Add(_buttonsHeldDownTillWallBreaks);
    // }

    // HELP IS THIS CORRECT DONE TO NETWORK? particularly the Destroyed part.
    public void ButtonHeldDown()
    {
        // if (IsOwner)
        // {
            CurrentButtonsHeld++;
            ClampButtonsHeld();
            if (CurrentButtonsHeld >= ButtonsHeldDownTillWallBreaks)
            {
                Destroy(gameObject);
            }
        // }
    }
    
    public void ButtonReleased()
    {
        // if (IsOwner)
        // {
            CurrentButtonsHeld--;
            ClampButtonsHeld();
        // }
    }

    private void ClampButtonsHeld()
    {
        CurrentButtonsHeld = Mathf.Clamp(CurrentButtonsHeld, 0, ButtonsHeldDownTillWallBreaks);
    }
}
