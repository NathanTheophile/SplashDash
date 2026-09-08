#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyCallback, MyStruct
#endregion

using UnityEngine;

public class Fish : MonoBehaviour
{
    #region _________________________/ REFERENCES
    [SerializeField] private PlayerMovement _PlayerMovementSystem;
    [SerializeField] private PlayerDash _PlayerDashSystem;
    [SerializeField] private PlayerPhysics2D _PlayerPhysicsSystem;
    [SerializeField] private PlayerPhysicsStates _PlayerPhysicsStates;
    private InputManager _Input;

    #endregion

    #region _________________________/ STATE VALUES

    public bool OnWater => _PlayerPhysicsSystem != null && _PlayerPhysicsSystem.IsOnWater;
    public bool IsDashing => _PlayerDashSystem != null && _PlayerDashSystem.IsDashing;
    public bool IsStunned { get; private set; }
    public bool CanControl => isActiveAndEnabled && !IsStunned && !IsDashing;
    public PlayerStates State => 
        IsStunned ? PlayerStates.STUNNED :
        IsDashing ? PlayerStates.IS_DASHING :
        OnWater ?   PlayerStates.ON_WATER : 
                    PlayerStates.ON_SAND;
                    
    public PlayerStats CurrentStats => GetStats(State);

    #endregion

    #region _________________________| INIT

    public PlayerStats GetStats(PlayerStates state) => _PlayerPhysicsStates != null
        ? _PlayerPhysicsStates.GetStats(state) : null;

    public void SetStunned(bool stunned)
    {
        IsStunned = stunned;
        if (!stunned) return;
        if (_PlayerMovementSystem != null) _PlayerMovementSystem.SetMoveInput(Vector2.zero);
    }

    private void OnEnable() => BindInput();

    #endregion

    #region _________________________| GAME FLOW METHODS

    private void Start()
    {
        if (_PlayerMovementSystem != null && _PlayerDashSystem != null && _PlayerPhysicsSystem != null &&
            GetStats(PlayerStates.ON_SAND) != null && GetStats(PlayerStates.ON_WATER) != null &&
            GetStats(PlayerStates.STUNNED) != null && GetStats(PlayerStates.IS_DASHING) != null) return;
        enabled = false;
    }

    private void Update()
    {
        BindInput();
        Vector2 moveDirection = Vector2.zero;
        if (CanControl && _Input != null)
            moveDirection = _Input.axis;

        _PlayerMovementSystem.SetMoveInput(moveDirection);
    }

    private void BindInput()
    {
        if (_Input == InputManager.Instance) return;
        if (_Input != null) _Input.JumpPressed -= Jump;
        _Input = InputManager.Instance;
        if (_Input != null) _Input.JumpPressed += Jump;
    }

    private void Jump()
    {
        if (isActiveAndEnabled && _PlayerDashSystem != null) _PlayerDashSystem.TryDash();
    }

    private void OnDisable()
    {
        if (_Input != null) _Input.JumpPressed -= Jump;
        _Input = null;
        IsStunned = false;
        if (_PlayerMovementSystem != null) _PlayerMovementSystem.ClearInput();
        if (_PlayerDashSystem != null) _PlayerDashSystem.ResetDash();
    }

    #endregion
}
