using UnityEngine;
using UnityEngine.Events;

public class ParticleTimer : MonoBehaviour
{
    [SerializeField] private float _timeBetweenParticles = 0.25f;
    public UnityEvent SpawnParticleEvent;
    private float _timer;

    private void Update()
    {
        if (_timer >= _timeBetweenParticles)
        {
            SpawnParticleEvent?.Invoke();
            _timer = 0;
            return;
        }
        _timer += Time.deltaTime;
    }
}
