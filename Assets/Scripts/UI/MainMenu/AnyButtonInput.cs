using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class AnyButtonInput : MonoBehaviour
{
    [SerializeField] private UnityEvent _buttonEvent;

    private void Start()
    {
        InputSystem.onAnyButtonPress.Call(OnCall);
    }

    private void OnCall(InputControl control)
    {
        _buttonEvent?.Invoke();
    }
}
