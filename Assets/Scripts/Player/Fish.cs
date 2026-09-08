using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private PlayerTrailEmitter _PlayerTrailEmitter;
    private Rigidbody2D _rigidBody;
    [SerializeField] private PlayerPhysicsStates _physicsStates;
    private PlayerStats _stats;

    private PlayerStates _state;
    public PlayerStates State 
    { 
        get => _state; 
        set
        {
            _stats = _physicsStates.states[(int)value];
            _state = value;
        }
    }

    private bool _canJump = true;
    private bool _onWater;
    private bool _isDashing;

    private float dashDuration => _stats.jumpCooldown * 0.5f;

    private Vector2 _moveDirection;

    public bool OnWater => _onWater;
    public bool IsDashing => _isDashing;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _PlayerTrailEmitter = GetComponent<PlayerTrailEmitter>();
    }

    private void Start()
    {
        InputManager.Instance.JumpPressed += Jump;
        State = PlayerStates.ON_SAND;
    }

    void Update()
    {
        _moveDirection = InputManager.Instance.axis;
        RotateTowards(transform.up, new Vector3(_moveDirection.x, _moveDirection.y), _stats.rotationSpeed * Time.deltaTime);
        _PlayerTrailEmitter.UpdateTrail(transform.position);

        _rigidBody.linearVelocity = Vector2.ClampMagnitude(_rigidBody.linearVelocity, _stats.maxMoveSpeed);
    }

    private void FixedUpdate()
    {
        if (_moveDirection != Vector2.zero)
        {
            _rigidBody.linearDamping = 0;
            _rigidBody.angularVelocity = 0;
            _rigidBody.AddForce(_moveDirection * _stats.moveSpeed, ForceMode2D.Force);
        }
        else _rigidBody.linearDamping = _stats.friction;
    }

    private void Jump()
    {
        if (!_canJump) return;
        _rigidBody.AddForce(transform.up * _stats.jumpForce, ForceMode2D.Impulse);
        State = PlayerStates.IS_DASHING;
        _canJump = false;
        _isDashing = true;
        _PlayerTrailEmitter.BeginDash(transform.up);
        StartCoroutine(JumpCooldownCoroutine());
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator JumpCooldownCoroutine()
    {
        float lJumpCdTimer = _stats.jumpCooldown;
        while (lJumpCdTimer > 0)
        {
            lJumpCdTimer -= Time.deltaTime;
            yield return null;
        }
        _canJump = true;
    }

    private IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;
        _PlayerTrailEmitter.EndDash();
    }

    internal void SetOnWater(bool pOnWater)
    {
        _onWater = pOnWater;
        State = _onWater ? PlayerStates.ON_WATER : PlayerStates.ON_SAND;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.JumpPressed -= Jump;
    }

    private void RotateTowards(Vector3 pFrom, Vector3 pTo, float pMaxAngle)
    {
        float lAngle = Vector3.SignedAngle(pFrom, pTo, transform.forward);
        transform.rotation *= Quaternion.AngleAxis(
            lAngle >= 0 ? Mathf.Clamp(pMaxAngle, 0, lAngle) : Mathf.Clamp(-pMaxAngle, lAngle, 0),
            transform.forward
            );
    }
}
