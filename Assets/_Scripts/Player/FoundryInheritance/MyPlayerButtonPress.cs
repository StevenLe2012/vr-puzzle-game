using System;
using System.Collections;
using System.Collections.Generic;
using Foundry;
using Foundry.Networking;
using UnityEngine;
// using Avatar = Foundry.Avatar;

[RequireComponent(typeof(CharacterController))]
public class MyPlayerButtonPress : NetworkComponent
{
    // [Header("Avatar")]
    // public Avatar avatar;

    public bool movementEnabled = true;
    [SerializeField] private float verticalMovementSpeed = 3f;
    [SerializeField] private float verticalScaleAmount = 3f;
    
    private IPlayerControlRig controlRig;
    private CharacterController controller;
    
    private NetworkProperty<TrackingMode> trackingMode = new NetworkProperty<TrackingMode>(TrackingMode.OnePoint);
    
    private NetworkProperty<Vector3> virtualScale = new(Vector3.zero);
    
    public override void RegisterProperties(List<INetworkProperty> properties)
    {
        properties.Add(trackingMode);
        properties.Add(virtualScale);
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // If this is an offline local player we can just borrow the rig
        if (!NetworkManager.instance)
            BorrowControlRig();
    }
    
    public void Update()
    {
        if(IsOwner)
            Move(GetVerticalMovement(), Time.deltaTime);
    }
    
    public override void OnConnected()
    {
        if(IsOwner)
            BorrowControlRig();
    }
    
    private void LoadControlRig()
    {
        controlRig.transform.SetParent(transform, false);
        controlRig.transform.localPosition = Vector3.zero;
        controlRig.transform.localRotation = Quaternion.identity;
            
        //If this is a desktop rig, change the camera mode
        if(controlRig is DesktopControlRig)
            ((DesktopControlRig)controlRig).SetCameraMode(DesktopControlRig.CameraMode.Look);
            
        trackingMode.Value = controlRig.GetTrackingMode();
    }
    
    public void BorrowControlRig()
    {
        var rigManager = FoundryApp.GetService<IPlayerRigManager>();
        if (rigManager.Rig != null)
        {
            controlRig = rigManager.BorrowPlayerRig();
            LoadControlRig();
        }
        else
        {
            rigManager.PlayerRigCreated += rig =>
            {
                controlRig = rigManager.BorrowPlayerRig();
                LoadControlRig();
            };
        }
    }


    public void Move(Vector3 movement, float deltaTime)
    {
        // controller.Move(movement * deltaTime);
        // virtualVelocity.Value = movement;
        controlRig.transform.localScale += movement * deltaTime;
        virtualScale.Value = movement;
    }
    
    public Vector3 GetVerticalMovement()
    {
        if (controlRig == null)
            return Vector3.zero;
        var buttonInput = ButtonInputManager.Instance;
        bool rightPrimaryButtonPressed = buttonInput.isRightPrimaryButtonPressed;
        bool rightSecondaryButtonPressed = buttonInput.isRightSecondaryButtonPressed;
        // Vector3 movement = new Vector3(0, 0, 0);
        Vector3 scaleAmount = Vector3.zero;
        // Quaternion movementReferenceRot = Quaternion.identity;

        if (rightPrimaryButtonPressed)
        {
            // movement = new Vector3(0, verticalMovementSpeed, 0);
            scaleAmount = ChangeScale(verticalScaleAmount);
        }
        if (rightSecondaryButtonPressed)
        {
            // movement = new Vector3(0, -verticalMovementSpeed, 0);
            scaleAmount = ChangeScale(-verticalScaleAmount);
        }
        
        if (rightPrimaryButtonPressed || rightSecondaryButtonPressed)
        {
            print("scaleAmount: " + scaleAmount);
        }
        // movement = Quaternion.AngleAxis(movementReferenceRot.eulerAngles.y, Vector3.up) * movement;
        return scaleAmount;
    }


    public Vector3 ChangeScale(float scaleAmount)
    {
        if (controlRig == null) throw new Exception("Could not find control rig");
        // var curScale = controlRig.transform.localScale;
        // print(scaleAmount);
        // return new Vector3(curScale.x + scaleAmount, curScale.y + scaleAmount, curScale.z + scaleAmount);
        return new Vector3(scaleAmount, scaleAmount, scaleAmount);
    }
}
