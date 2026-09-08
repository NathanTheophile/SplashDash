using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class AnyButtonInput : MonoBehaviour
{
    public UnityEvent OnButtonEvent;
    [SerializeField] private bool _oneShot = true;

    private IDisposable _subscription;

    private void OnEnable()
    {
        _subscription = InputSystem.onAnyButtonPress.Call(OnCall);
    }

    private void OnDisable()
    {
        _subscription?.Dispose();
        _subscription = null;
    }

    private void OnCall(InputControl control)
    {
        OnButtonEvent?.Invoke();

        if (_oneShot)
        {
            enabled = false;
        }
    }
}
