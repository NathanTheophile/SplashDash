using UnityEngine;

public class Fish : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    [SerializeField] private float _speed = 5;
    [SerializeField] private float _friction = 2;
    [SerializeField] private float _maxMoveSpeed = 5;
    [SerializeField] private float _rotationSpeed = 1;

    private Vector2 _moveDirection;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _moveDirection = InputManager.Instance.axis;
        RotateTowards(transform.up, new Vector3(_moveDirection.x, _moveDirection.y), _rotationSpeed * Time.deltaTime);

        _rigidBody.linearVelocity = Vector2.ClampMagnitude(_rigidBody.linearVelocity, _maxMoveSpeed);
    }

    private void FixedUpdate()
    {
        if (_moveDirection != Vector2.zero)
        {
            _rigidBody.linearDamping = 0;
            _rigidBody.AddForce(_moveDirection * _speed, ForceMode2D.Force);
        }
        else _rigidBody.linearDamping = _friction;
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
