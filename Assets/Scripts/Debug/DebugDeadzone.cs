using UnityEngine;

public class DebugDeadzone : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Fish fishprefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Fish>() == null) return;
        Fish replacement = Instantiate(fishprefab, spawnPoint.position, spawnPoint.rotation);
        replacement.GetComponent<PlayerTrailEmitter>()?.SetTrailManager(FindFirstObjectByType<TrailManager>());
        Destroy(other.gameObject);
    }
}
