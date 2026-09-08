using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
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
    [SerializeField] private PlayerStates _startState; // FOR TESTING PURPOSES

    private Vector2 _moveDirection;
    private bool _canJump = true;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InputManager.Instance.JumpPressed += Jump;
        State = _startState;
    }

    void Update()
    {
        _moveDirection = InputManager.Instance.axis;
        RotateTowards(transform.up, new Vector3(_moveDirection.x, _moveDirection.y), _stats.rotationSpeed * Time.deltaTime);

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
        _canJump = false;
        StartCoroutine(JumpCooldownCoroutine());
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

    private void RotateTowards(Vector3 pFrom, Vector3 pTo, float pMaxAngle)
    {
        float lAngle = Vector3.SignedAngle(pFrom, pTo, transform.forward);
        transform.rotation *= Quaternion.AngleAxis(
            lAngle >= 0 ? Mathf.Clamp(pMaxAngle, 0, lAngle) : Mathf.Clamp(-pMaxAngle, lAngle, 0),
            transform.forward
            );
    }
}
