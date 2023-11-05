using System;
using System.Collections;
using System.Collections.Generic;
using Foundry;
using Foundry.Networking;
using UnityEngine;
// using Avatar = Foundry.Avatar;

public class PlayerScale : NetworkComponent
{
    // [Header("Avatar")]
    // public Avatar avatar;

    public bool movementEnabled = true;
    [SerializeField] private float verticalMovementSpeed = 3f;
    [SerializeField] private float verticalScaleAmount = 3f;
    
    private ButtonInputManager _buttonInput;
    private IPlayerControlRig controlRig;
    
    private NetworkProperty<TrackingMode> trackingMode = new NetworkProperty<TrackingMode>(TrackingMode.OnePoint);
    
    private NetworkProperty<Vector3> virtualScale = new(Vector3.zero);
    private CharacterController controller;

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
        _buttonInput = ButtonInputManager.Instance;
        if (!NetworkManager.instance)
            BorrowControlRig();
        
        controller.Move(new Vector3(0, 10, 0));
        virtualScale.Value = new Vector3(0, 10, 0);
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
        controller.Move(movement * deltaTime);
        virtualScale.Value = movement;
        // controlRig.transform.localScale += movement * deltaTime;
        // virtualScale.Value = movement;
    }
    
    public Vector3 GetVerticalMovement()
    {
        if (controlRig == null)
            return Vector3.zero;
        bool rightPrimaryButtonPressed = _buttonInput.isRightPrimaryButtonPressed;
        bool rightSecondaryButtonPressed = _buttonInput.isRightSecondaryButtonPressed;
        Vector3 scaleAmount = Vector3.zero;

        if (rightPrimaryButtonPressed)
        {
            // scaleAmount = new Vector3(verticalScaleAmount, verticalScaleAmount, verticalScaleAmount);
            scaleAmount = new Vector3(0, verticalScaleAmount, 0);
        }
        if (rightSecondaryButtonPressed)
        {
            // scaleAmount = new Vector3(-verticalScaleAmount, -verticalScaleAmount, -verticalScaleAmount);
            scaleAmount = new Vector3(0, -verticalScaleAmount, 0);
        }
        
        return scaleAmount;
    }
}
