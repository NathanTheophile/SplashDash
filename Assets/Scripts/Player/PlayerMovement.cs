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
    private Vector2 pendingImpulse;

    public PlayerStats CurrentStats => _Player != null ? _Player.CurrentStats : null;
    public PlayerStats DashStats => _Player != null ? _Player.GetStats(PlayerStates.IS_DASHING) : null;
    public bool CanControl => isActiveAndEnabled && _Player != null && _Player.CanControl;

    #endregion

    #region _________________________| INIT

    private void Awake()
    {
        if (_PlayerBody != null && _Player != null && CurrentStats != null) return;
        enabled = false;
    }

    #endregion

    #region _________________________| PHYSIC METHODS

    public void SetMoveInput(Vector2 direction) => moveInput = CanControl ? direction : Vector2.zero;

    public void QueueImpulse(Vector2 impulse)
    {
        if (isActiveAndEnabled) pendingImpulse += impulse;
    }

    public void ClearInput()
    {
        moveInput = Vector2.zero;
        pendingImpulse = Vector2.zero;
    }

    private void OnDisable() => ClearInput();

    private void FixedUpdate()
    {
        PlayerStats stats = CurrentStats;
        if (stats == null) return;
        Vector2 direction = CanControl ? moveInput : Vector2.zero;
        if (direction != Vector2.zero)
        {
            RotateTowards(transform.up, new Vector3(direction.x, direction.y),
                            stats.rotationSpeed * Time.fixedDeltaTime);
            _PlayerBody.linearDamping = 0;
            _PlayerBody.angularVelocity = 0;
        }
        else _PlayerBody.linearDamping = stats.friction;

        _PlayerBody.linearVelocity = Vector2.ClampMagnitude(_PlayerBody.linearVelocity, stats.maxMoveSpeed);
        _PlayerBody.AddForce(direction * stats.moveSpeed, ForceMode2D.Force);
        if (pendingImpulse != Vector2.zero)
        {
            _PlayerBody.AddForce(pendingImpulse, ForceMode2D.Impulse);
            pendingImpulse = Vector2.zero;
        }
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
