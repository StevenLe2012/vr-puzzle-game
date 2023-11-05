using System;
using System.Collections;
using System.Collections.Generic;
using Foundry;
using Foundry.Networking;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerJump : NetworkComponent
{
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private LayerMask groundLayers;

    private float _gravity = Physics.gravity.y;
    private Vector3 _movement;
    private ButtonInputManager _buttonInput;
    private CharacterController controller;
    
    // private NetworkProperty<TrackingMode> trackingMode = new NetworkProperty<TrackingMode>(TrackingMode.OnePoint);
    private NetworkProperty<Vector3> virtualVelocity = new(Vector3.zero);

    public override void RegisterProperties(List<INetworkProperty> properties)
    {
        // properties.Add(trackingMode);
        properties.Add(virtualVelocity);
    }
    
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _buttonInput = ButtonInputManager.Instance;
    }

    private void Update()
    {
        bool _isGrounded = IsGrounded();
        if (_buttonInput.isRightPrimaryButtonPressed && _isGrounded)
        {
            Jump();
        }

        _movement.y += _gravity * Time.deltaTime;
        Move(_movement, Time.deltaTime);
    }

    private void Move(Vector3 movement, float deltaTime)
    {
        controller.Move(movement * deltaTime);
        virtualVelocity.Value = movement;
    }

    private void Jump()
    {
        _movement.y = Mathf.Sqrt(jumpHeight * -3.0f * _gravity);
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position, 0.05f, groundLayers);
    }
}
