using UnityEngine;

public class PlayerTrailEmitter : MonoBehaviour
{
    [SerializeField] private TrailManager trailManager;
    [SerializeField] private float trailSpacing = 0.2f;

    private Vector2 lastPosition;
    private DashTrail activeDash;
    private bool isDashing;

    private void Start()
    {
        lastPosition = transform.position;
    }

    public void UpdateTrail(Vector2 position)
    {
        if (isDashing)
        {
            activeDash.SetEnd(position);
            return;
        }

        if (Vector2.Distance(lastPosition, position) < trailSpacing)
            return;

        trailManager.CreateWaterTrail(lastPosition, position);
        lastPosition = position;
    }

    public void BeginDash(Vector2 direction)
    {
        isDashing = true;

        activeDash = trailManager.CreateDashTrail(
            transform.position,
            direction
        );
    }

    public void EndDash()
    {
        isDashing = false;
        activeDash = null;

        lastPosition = transform.position;
    }
}
