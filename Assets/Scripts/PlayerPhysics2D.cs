using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics2D : MonoBehaviour
{
    private int waterTrailContacts;

    private readonly List<DashTrail> dashTrails = new();

    public bool IsOnWaterTrail => waterTrailContacts > 0;

    public DashTrail CurrentDashTrail =>
        dashTrails.Count > 0 ? dashTrails[^1] : null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WaterTrail"))
            waterTrailContacts++;

        if (other.CompareTag("DashTrail") &&
            other.TryGetComponent(out DashTrail dashTrail))
        {
            dashTrails.Add(dashTrail);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("WaterTrail"))
            waterTrailContacts = Mathf.Max(0, waterTrailContacts - 1);

        if (other.CompareTag("DashTrail") &&
            other.TryGetComponent(out DashTrail dashTrail))
        {
            dashTrails.Remove(dashTrail);
        }
    }

    public float GetCurrentAlignment(Vector2 input)
    {
        DashTrail trail = CurrentDashTrail;

        if (trail == null || input == Vector2.zero)
            return 0f;

        return Vector2.Dot(
            input.normalized,
            trail.Direction
        );
    }

    public void ClearContacts()
    {
        waterTrailContacts = 0;
        dashTrails.Clear();
    }
}