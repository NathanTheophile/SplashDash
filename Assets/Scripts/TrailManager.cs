using System.Collections.Generic;
using UnityEngine;

public class TrailManager : MonoBehaviour
{
    [SerializeField] private WaterTrail waterTrailPrefab;
    [SerializeField] private DashTrail dashTrailPrefab;

    private readonly List<GameObject> trails = new();

    public void CreateWaterTrail(Vector2 start, Vector2 end)
    {
        
    }

    public DashTrail CreateDashTrail(Vector2 position, Vector2 direction)
    {
        DashTrail trail = Instantiate(dashTrailPrefab);
        trail.Begin(position, direction);

        trails.Add(trail.gameObject);

        return trail;
    }

    public void ClearTrails()
    {
        foreach (GameObject trail in trails)
        {
            if (trail != null)
                Destroy(trail);
        }

        trails.Clear();
    }
}