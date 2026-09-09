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

    public bool IsOnWater => _PlayerPhysicsSystem.IsOnWater;
    public bool IsDashing => _PlayerDashSystem.IsDashing;
    public bool IsStunned { get; private set; }
    public Vector2 CurrentDirection => _PlayerPhysicsSystem.CurrentDirection; 
    public bool AreControlsEnabled => !IsStunned && !IsDashing;
    public PlayerStates State => GetState();
                    
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

    private PlayerStates GetState()
    {
        if (IsStunned) return PlayerStates.STUNNED;
        if (IsDashing) return PlayerStates.IS_DASHING;
        if (IsOnWater) return PlayerStates.ON_WATER;
        return PlayerStates.ON_SAND;
    }

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

    private void Update()
    {
        BindInput();
        Vector2 moveDirection = Vector2.zero;
        if (AreControlsEnabled && _Input != null)
            moveDirection = _Input.axis;

        _PlayerMovementSystem.SetMoveInput(moveDirection);
    }

    #endregion

    #region _________________________| INPUTS

    private void BindInput()
    {
        if (_Input == InputManager.Instance) return;

        if (_Input != null)
        {
            _Input.DashPressed -= PressDash;
            _Input.DashReleased -= ReleaseDash;
        }

        _Input = InputManager.Instance;

        if (_Input != null)
        {
            _Input.DashPressed += PressDash;
            _Input.DashReleased += ReleaseDash;
        }
    }

    private void PressDash()
    {
        if (AreControlsEnabled)
            _PlayerDashSystem.PressDash();
    }

    private void ReleaseDash()
    {
        _PlayerDashSystem.ReleaseDash();
    }

    private void OnDisable()
    {
        if (_Input != null)
        {
            _Input.DashPressed -= PressDash;
            _Input.DashReleased -= ReleaseDash;
        }        
        _Input = null;
        IsStunned = false;
        _PlayerMovementSystem.ClearInput();
        _PlayerDashSystem.ResetDash();
    }

    #endregion
}
