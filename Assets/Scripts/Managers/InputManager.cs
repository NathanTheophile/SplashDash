using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private InputSystem_Actions _inputActions;

    public Vector2 axis;

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
    }

    private void MoveAction(InputAction.CallbackContext pContext)
    {
        axis = pContext.ReadValue<Vector2>();
    }
}
