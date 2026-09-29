using UnityEngine;

public class FinAnimator : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxAngle = 30f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    [SerializeField] private SpriteRenderer _renderer;

    private float phase;
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.localRotation;
    }

    public void SetSkin(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    private void Update()
    {
        // L'accumulation de la phase garantit une transition fluide lors du changement de vitesse
        phase += Time.deltaTime * speed;

        float currentAngle = Mathf.Sin(phase) * maxAngle;
        transform.localRotation = initialRotation * Quaternion.Euler(rotationAxis * currentAngle);
    }
}
