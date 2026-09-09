using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private InputSystem_Actions _inputActions;

    public Vector2 axis;
    
    public event Action DashPressed;
    public event Action DashReleased;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        _inputActions = new();
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += MoveAction;
        _inputActions.Player.Move.canceled += MoveAction;
        _inputActions.Player.Jump.performed += DashPressedAction;
        _inputActions.Player.Jump.canceled += DashReleasedAction;
    }

    private void MoveAction(InputAction.CallbackContext pContext)
    {
        axis = pContext.ReadValue<Vector2>();
    }

    private void DashPressedAction(InputAction.CallbackContext pContext)
    {
        DashPressed?.Invoke();
    }
    
    private void DashReleasedAction(InputAction.CallbackContext pContext)
    {
        DashReleased?.Invoke();
    }
}
