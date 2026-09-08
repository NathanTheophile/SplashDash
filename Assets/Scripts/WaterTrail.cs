using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class WaterTrail : MonoBehaviour
{
    [SerializeField] private Color _DebugColor = Color.cyan;

    private void Awake()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void Update()
    {
        CircleCollider2D lCollider = GetComponent<CircleCollider2D>();
        Vector3 lCenter = lCollider.transform.TransformPoint(lCollider.offset);
        float lRadius = lCollider.radius * transform.lossyScale.x;
        const int lSegments = 16;

        for (int i = 0; i < lSegments; i++)
        {
            float lCurrentAngle = i * Mathf.PI * 2f / lSegments;
            float lNextAngle = (i + 1) * Mathf.PI * 2f / lSegments;
            Vector3 lCurrentPoint = lCenter + new Vector3(Mathf.Cos(lCurrentAngle), Mathf.Sin(lCurrentAngle)) * lRadius;
            Vector3 lNextPoint = lCenter + new Vector3(Mathf.Cos(lNextAngle), Mathf.Sin(lNextAngle)) * lRadius;

            Debug.DrawLine(lCurrentPoint, lNextPoint, _DebugColor);
        }
    }
}
