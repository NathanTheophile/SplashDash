#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyCallback, MyStruct
#endregion

using UnityEngine;
using UnityEngine.InputSystem;

public class Fish : MonoBehaviour
{
    #region _________________________/ REFERENCES
    [SerializeField] private PlayerMovement _PlayerMovementSystem;
    [SerializeField] private PlayerDash _PlayerDashSystem;
    [SerializeField] private PlayerPhysics2D _PlayerPhysicsSystem;
    [SerializeField] private PlayerPhysicsStates _PlayerPhysicsStates;

    #endregion

    #region _________________________/ STATE VALUES

    public bool IsOnWater => _PlayerPhysicsSystem.IsOnWater;
    public bool IsDashing => _PlayerDashSystem.IsDashing;
    public bool IsStunned { get; private set; }
    public bool AreControlsEnabled => !IsStunned && !IsDashing;
    public PlayerStates State => 
        IsStunned ? PlayerStates.STUNNED :
        IsDashing ? PlayerStates.IS_DASHING :
        IsOnWater ?   PlayerStates.ON_WATER : 
                    PlayerStates.ON_SAND;
                    
    public PlayerStats CurrentStats => GetStats(State);

    #endregion

    #region _________________________| INIT

    private void Awake()
    {
        if (_PlayerMovementSystem == null)
            _PlayerMovementSystem = GetComponent<PlayerMovement>();

        if (_PlayerDashSystem == null)
            _PlayerDashSystem = GetComponent<PlayerDash>();

        if (_PlayerPhysicsSystem == null)
            _PlayerPhysicsSystem = GetComponent<PlayerPhysics2D>();
    }

    public PlayerStats GetStats(PlayerStates state) => _PlayerPhysicsStates.GetStats(state);

    public void SetStunned(bool stunned)
    {
        IsStunned = stunned;
        if (!stunned) return;
        _PlayerDashSystem.CancelCharge();
        _PlayerMovementSystem.SetMoveInput(Vector2.zero);
    }

    private void OnEnable() => BindInput();

    #endregion

    #region _________________________| UNITY

    private Vector2 _moveDirection;

    private void Update()
    {
        //BindInput();
        //_moveDirection = Vector2.zero;
        //if (AreControlsEnabled && _Input != null)
        //    moveDirection = _Input.axis;

        _PlayerMovementSystem.SetMoveInput(_moveDirection);
    }

    #endregion

    #region _________________________| INPUTS

    private void BindInput()
    {
        //if (_Input == InputManager.Instance) return;

        //if (_Input != null)
        //{
        //    _Input.DashPressed -= PressDash;
        //    _Input.DashReleased -= ReleaseDash;
        //}

        //_Input = InputManager.Instance;

        //if (_Input != null)
        //{
        //    _Input.DashPressed += PressDash;
        //    _Input.DashReleased += ReleaseDash;
        //}
    }

    public void HandleMove(InputAction.CallbackContext pContext)
    {
        _moveDirection = pContext.ReadValue<Vector2>();
    }

    public void PressDash(InputAction.CallbackContext pContext)
    {
        if (AreControlsEnabled && pContext.performed) _PlayerDashSystem.PressDash();
        else if (pContext.canceled) _PlayerDashSystem.ReleaseDash();
    }

    private void OnDisable()
    {
        //if (_Input != null)
        //{
        //    _Input.DashPressed -= PressDash;
        //    _Input.DashReleased -= ReleaseDash;
        //}        
        //_Input = null;
        IsStunned = false;
        _PlayerMovementSystem.ClearInput();
        _PlayerDashSystem.ResetDash();
    }

    #endregion
}
