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
    [SerializeField] private Fish _Player;

    #endregion

    #region _________________________/ TUNING VALUES
    #endregion

    #region _________________________/ RUNTIME VALUES
    private Vector2 moveInput;
    private Vector2 _DashDirection;
    private float _DashSpeed;
    private float _ChargeSpeedMultiplier = 1f;
    private bool _IsDashMoving;
    [SerializeField] private float _MinChargeSpeed;

    public PlayerStats CurrentStats => _Player.CurrentStats;
    public PlayerStats DashStats => _Player.GetStats(PlayerStates.IS_DASHING);
    public bool AreControlsEnabled => _Player.AreControlsEnabled;

    #endregion

    #region _________________________| INIT

    private void Awake()
    {
        if (_PlayerBody == null)
            _PlayerBody = GetComponent<Rigidbody2D>();

        if (_Player == null)
            _Player = GetComponent<Fish>();
    }

    #endregion

    #region _________________________| PHYSIC METHODS

    public void SetMoveInput(Vector2 direction) => moveInput = AreControlsEnabled ? direction : Vector2.zero;

    public void BeginDash(Vector2 pDirection, float pSpeed)
    {
        _DashDirection = pDirection.normalized;
        _DashSpeed = pSpeed;
        _IsDashMoving = true;
        _PlayerBody.angularVelocity = 0;
    }

    public void SetChargeSpeed(float pChargeRatio) => _ChargeSpeedMultiplier = 1f - pChargeRatio;
    public void ResetChargeSpeed() => _ChargeSpeedMultiplier = 1f;

    public void EndDash()
    {
        _IsDashMoving = false;
        _DashDirection = Vector2.zero;
        _DashSpeed = 0f;
    }

    public void ClearInput()
    {
        moveInput = Vector2.zero;
        EndDash();
    }

    private void OnDisable() => ClearInput();

    private void FixedUpdate()
    {
        PlayerStats stats = CurrentStats;

        if (_IsDashMoving)
        {
            _PlayerBody.linearDamping = 0;
            _PlayerBody.linearVelocity = _DashDirection * _DashSpeed;
            return;
        }

        Vector2 direction = AreControlsEnabled ? moveInput : Vector2.zero;
        if (direction != Vector2.zero)
        {
            RotateTowards(transform.up, new Vector3(direction.x, direction.y),
                            stats.rotationSpeed * Time.fixedDeltaTime);
            _PlayerBody.linearDamping = 0;
            _PlayerBody.angularVelocity = 0;
        }
        else _PlayerBody.linearDamping = stats.friction;

        _PlayerBody.linearVelocity = Vector2.ClampMagnitude(_PlayerBody.linearVelocity, stats.maxMoveSpeed * Mathf.Clamp(_ChargeSpeedMultiplier, _MinChargeSpeed, _ChargeSpeedMultiplier));
        _PlayerBody.AddForce(direction * stats.moveSpeed * _ChargeSpeedMultiplier, ForceMode2D.Force);
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
