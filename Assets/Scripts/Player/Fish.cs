using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private PlayerTrailEmitter _PlayerTrailEmitter;
    private Rigidbody2D _rigidBody;

    [SerializeField] private float _speed = 5;
    [SerializeField] private float _friction = 0;
    [SerializeField] private float _maxMoveSpeed = 5;
    [SerializeField] private float _rotationSpeed = 1;

    [SerializeField] private float _jumpForce = 10;
    [SerializeField, Min(1f)] private float _waterDashMultiplier = 1.5f;
    [SerializeField] private float _jumpCooldown = 3;
    [SerializeField, Min(0.01f)] private float _dashDuration = 0.2f;
    private bool _canJump = true;
    private bool _onWater;
    private bool _isDashing;

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
    }

    void Update()
    {
        _moveDirection = InputManager.Instance.axis;
        RotateTowards(transform.up, new Vector3(_moveDirection.x, _moveDirection.y), _rotationSpeed * Time.deltaTime);
        _PlayerTrailEmitter.UpdateTrail(transform.position);

        _rigidBody.linearVelocity = Vector2.ClampMagnitude(_rigidBody.linearVelocity, _maxMoveSpeed);
    }

    private void FixedUpdate()
    {
        if (_moveDirection != Vector2.zero)
        {
            _rigidBody.linearDamping = 0;
            _rigidBody.angularVelocity = 0;
            _rigidBody.AddForce(_moveDirection * _speed, ForceMode2D.Force);
        }
        else _rigidBody.linearDamping = _friction;
    }

    private void Jump()
    {
        if (!_canJump) return;
        float lDashForce = _jumpForce * (_onWater ? _waterDashMultiplier : 1f);
        _rigidBody.AddForce(transform.up * lDashForce, ForceMode2D.Impulse);
        _canJump = false;
        _isDashing = true;
        _PlayerTrailEmitter.BeginDash(transform.up);
        StartCoroutine(JumpCooldownCoroutine());
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator JumpCooldownCoroutine()
    {
        float lJumpCdTimer = _jumpCooldown;
        while(lJumpCdTimer > 0)
        {
            lJumpCdTimer -= Time.deltaTime;
            yield return null;
        }
        _canJump = true;
    }

    private IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(_dashDuration);
        _isDashing = false;
        _PlayerTrailEmitter.EndDash();
    }

    internal void SetOnWater(bool pOnWater)
    {
        _onWater = pOnWater;
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
