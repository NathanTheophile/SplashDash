using UnityEngine;
using UnityEngine.Events;

//Gemini

public class UniversalColorOscillator : MonoBehaviour
{
    [Header("Couleurs")]
    [SerializeField] private Color colorA = Color.white;
    [SerializeField] private Color colorB = Color.black;

    [Header("Réglages")]
    [SerializeField] private float speed = 2f;

    [Header("Cible")]
    [SerializeField] private UnityEvent<Color> onColorChanged;

    private void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        Color currentColor = Color.Lerp(colorA, colorB, t);

        onColorChanged?.Invoke(currentColor);
    }
}
