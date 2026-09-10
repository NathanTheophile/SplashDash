#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyCallback, MyStruct
#endregion

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region _________________________/ REFERENCES
    [SerializeField] private Rigidbody2D _PlayerBody;
    [SerializeField] private Fish _Fish;

    #endregion

    #region _________________________/ TUNING VALUES
    [SerializeField, Min(0f)] private float _CurrentForce = 2.5f;
    #endregion

    #region _________________________/ RUNTIME VALUES
    private Vector2 _MoveInput;
    private Vector2 _DashDirection;
    private float _DashSpeed;
    private float _ChargeSpeedMultiplier = 1f;
    private bool _IsDashMoving;
    [SerializeField] private float _MinChargeSpeed;

    public PlayerStats CurrentStats => _Fish.CurrentStats;
    public PlayerStats DashStats => _Fish.GetStats(PlayerStates.IS_DASHING);
    public bool AreControlsEnabled => _Fish.AreControlsEnabled;

    #endregion

    #region _________________________| INIT

    private void Awake()
    {
        if (_PlayerBody == null)
            _PlayerBody = GetComponent<Rigidbody2D>();

        if (_Fish == null)
            _Fish = GetComponent<Fish>();
    }

    #endregion

    #region _________________________| PHYSIC METHODS

    public void SetMoveInput(Vector2 inputDirection)
    {
        _MoveInput = AreControlsEnabled ? inputDirection : Vector2.zero;

    }

    public void BeginDash(Vector2 pDirection, float pSpeed)
    {
        _DashDirection = pDirection.normalized;
        _DashSpeed = pSpeed;
        _IsDashMoving = true;
        _PlayerBody.angularVelocity = 0;
    }

    public void SetChargeSpeed(float pChargeRatio) => _ChargeSpeedMultiplier = Mathf.Clamp(1f - pChargeRatio, _MinChargeSpeed, 1f);
    public void ResetChargeSpeed() => _ChargeSpeedMultiplier = 1f;

    public void EndDash()
    {
        _IsDashMoving = false;
        _DashDirection = Vector2.zero;
        _DashSpeed = 0f;
    }

    public void ClearInput()
    {
        _MoveInput = Vector2.zero;
        EndDash();
    }

    private void OnDisable() => ClearInput();

    private void FixedUpdate()
    {
        PlayerStats movementStats = CurrentStats;

        if (_IsDashMoving)
        {
            _PlayerBody.linearDamping = 0;
            _PlayerBody.linearVelocity = _DashDirection * _DashSpeed;
            return;
        }

        Vector2 inputDirection = AreControlsEnabled ? _MoveInput : Vector2.zero;
        if (inputDirection != Vector2.zero)
        {
            RotateTowards(transform.up, new Vector3(inputDirection.x, inputDirection.y),
                            movementStats.rotationSpeed * Time.fixedDeltaTime);
            _PlayerBody.linearDamping = 0;
            _PlayerBody.angularVelocity = 0;
        }
        else _PlayerBody.linearDamping = movementStats.friction;

        _PlayerBody.linearVelocity = Vector2.ClampMagnitude(_PlayerBody.linearVelocity, movementStats.maxMoveSpeed * _ChargeSpeedMultiplier);
        _PlayerBody.AddForce(inputDirection * movementStats.moveSpeed * _ChargeSpeedMultiplier, ForceMode2D.Force);
        _PlayerBody.AddForce(_Fish.CurrentDirection * _CurrentForce, ForceMode2D.Force);
    }

    private void RotateTowards(Vector3 from, Vector3 to, float maxAngle)
    {
        float angle = Vector3.SignedAngle(from, to, transform.forward);
        transform.rotation *= Quaternion.AngleAxis(
            angle >= 0 ? Mathf.Clamp(maxAngle, 0, angle) : Mathf.Clamp(-maxAngle, angle, 0),
            transform.forward);
    }

    #endregion
}
