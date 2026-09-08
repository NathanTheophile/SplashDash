using UnityEngine;

public class DashTrail : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D capsule;

    private Vector2 startPosition;
    private Vector2 direction;

    public Vector2 Direction => direction;

    private void Awake()
    {
        if (capsule == null)
            capsule = GetComponent<CapsuleCollider2D>();
    }

    public void Begin(Vector2 position, Vector2 dashDirection)
    {
        startPosition = position;
        direction = dashDirection.normalized;

        transform.right = direction;

        SetEnd(position);
    }

    public void SetEnd(Vector2 position)
    {
        float distance = Vector2.Dot(position - startPosition, direction);
        distance = Mathf.Max(0f, distance);

        Vector2 endPosition = startPosition + direction * distance;

        transform.position = (startPosition + endPosition) / 2f;

        float width = capsule.size.y;
        capsule.size = new Vector2(distance + width, width);
    }
}