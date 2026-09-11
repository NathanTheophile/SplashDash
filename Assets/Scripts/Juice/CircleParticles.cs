using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CircleParticleSystem : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    [Header("Particle Settings")]
    [SerializeField] private int particleCount = 20;
    [SerializeField] private float radius = 3f;
    [SerializeField] private float _startSpeed = 6f;

    [Header("Rotation")]
    [SerializeField] private float rotationOffset = 0f;

    private ParticleSystem.Particle[] particles;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        CreateParticles();
    }

    public void CreateParticles()
    {
        _particleSystem.Emit(particleCount);

        particles = new ParticleSystem.Particle[particleCount];

        int count = _particleSystem.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            float angle = Random.value * 360f;

            float angleRadians = angle * Mathf.Deg2Rad;

            Vector2 position = new Vector2(
                Mathf.Cos(angleRadians),
                Mathf.Sin(angleRadians)
            ) * radius;

            particles[i].position = position;

            Vector2 directionToCenter = -position;

            float rotation = angle;
            print(rotation);
            particles[i].rotation = -rotation + rotationOffset - transform.rotation.eulerAngles.z;
            particles[i].velocity = position * _startSpeed;
        }

        _particleSystem.SetParticles(particles, count);
    }
}