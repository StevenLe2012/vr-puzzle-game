using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonInputManager : MonoBehaviour
{
    public static ButtonInputManager Instance;
    
    public InputActionProperty LeftPrimaryButtonPressed;
    public InputActionProperty LeftSecondaryButtonPressed;
    public InputActionProperty RightPrimaryButtonPressed;
    public InputActionProperty RightSecondaryButtonPressed;
    
    public bool isLeftPrimaryButtonPressed;
    public bool isLeftSecondaryButtonPressed;
    public bool isRightPrimaryButtonPressed;
    public bool isRightSecondaryButtonPressed;
    
    private void Awake()
    {
        if (Instance != null)
            enabled = false;
        else 
            Instance = this;
    }

    private void Start()
    {
        ActivateActionsInternal();
    }

    private void ActivateActionsInternal()
    {
        LeftPrimaryButtonPressed.action.Enable();
        LeftSecondaryButtonPressed.action.Enable();
        RightPrimaryButtonPressed.action.Enable();
        RightSecondaryButtonPressed.action.Enable();
    }

    private void Update()
    {
        isLeftPrimaryButtonPressed = LeftPrimaryButtonPressed.action.IsPressed();
        isLeftSecondaryButtonPressed = LeftSecondaryButtonPressed.action.IsPressed();
        isRightPrimaryButtonPressed = RightPrimaryButtonPressed.action.IsPressed();
        isRightSecondaryButtonPressed = RightSecondaryButtonPressed.action.IsPressed();
    }
    
}
