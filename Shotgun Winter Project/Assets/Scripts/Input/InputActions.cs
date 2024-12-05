using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputActions : MonoBehaviour
{
    public Vector2 MoveDirection { get; private set; }
    public Vector2 LookDirection { get; private set; }
        
    public bool JumpPressed { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool JumpHeld { get; private set; }
        
    // Use this to call Jump.WasPressedThisFrame() or Jump.WasReleasedThisFrame() or . Jump.IsPressed();
    public InputAction Jump {get; private set; }
        
    public bool InteractPressed { get; private set; }
    public bool FirePressed { get; private set; }
    public bool ResetPressed { get; private set; }
    
    private void Update()
    {
        MoveDirection = _inputSystem.Player.Move.ReadValue<Vector2>();
        LookDirection = _inputSystem.Player.Look.ReadValue<Vector2>();
        // Shortcut to Input Actions
        Jump = _inputSystem.Player.Jump;
            
        // Seperated Pressed, Released, Held input
        JumpPressed = _inputSystem.Player.Jump.WasPressedThisFrame();
        JumpReleased = _inputSystem.Player.Jump.WasReleasedThisFrame();
        JumpHeld = _inputSystem.Player.Jump.IsPressed();
            
        FirePressed = _inputSystem.Player.Shoot.WasPressedThisFrame();
        ResetPressed = _inputSystem.Player.Reset.WasPressedThisFrame();
    }
    
    #region Initialise Input Actions
    // Reference to the Input Action Map
    private InputSystem_Actions _inputSystem;
    private void Awake() => _inputSystem = new InputSystem_Actions();
    private void OnEnable() => _inputSystem.Enable();
    private void OnDisable() => _inputSystem.Disable();
    #endregion
}
