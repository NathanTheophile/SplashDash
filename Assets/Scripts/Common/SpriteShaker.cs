using UnityEngine;

public class SpriteShaker : MonoBehaviour
{
    [Header("Paramètres de Shake")]
    [Tooltip("Intensité globale (facteur multiplicateur de 0 à 1)")]
    [SerializeField] private float _intensity = 0f;

    [Tooltip("Vitesse / fréquence des secousses")]
    [SerializeField] private float _speed = 20f;

    [Tooltip("Distance maximale du décalage en unités Unity")]
    [SerializeField] private float _distance = 0.2f;

    private Vector3 _initialLocalPosition;
    private float _seedX;
    private float _seedY;
    private float _timer;

    private void Awake()
    {
        // Sauvegarde de la position locale de référence
        _initialLocalPosition = transform.localPosition;

        // Valeurs aléatoires pour séparer les axes X et Y
        _seedX = Random.Range(0f, 1000f);
        _seedY = Random.Range(1000f, 2000f);
    }

    private void Update()
    {
        // Accumulation du temps avec la vitesse réglable
        _timer += Time.deltaTime * _speed;

        // Génération du bruit (Perlin Noise) entre -1 et 1 sur chaque axe
        float noiseX = (Mathf.PerlinNoise(_seedX + _timer, 0f) * 2f) - 1f;
        float noiseY = (Mathf.PerlinNoise(0f, _seedY + _timer) * 2f) - 1f;

        // Calcul du décalage
        var offset = _distance * _intensity * new Vector3(noiseX, noiseY, 0f);

        // Application par rapport à la position initiale
        transform.localPosition = _initialLocalPosition + offset;
    }

    public void SetShake(float intensity)
    {
        _intensity = intensity;
    }

    public void ResetShake()
    {
        transform.localPosition = _initialLocalPosition;
    }

    /// <summary>
    /// Permet de redéfinir la position de base si votre sprite se déplace dans la scène.
    /// </summary>
    public void UpdateInitialPosition()
    {
        _initialLocalPosition = transform.localPosition;
    }
}