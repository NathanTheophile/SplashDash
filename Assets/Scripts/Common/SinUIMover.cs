using UnityEngine;

public class SinUIMover : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    public float speed;

    public Vector2 end;
    private Vector2 start;

    private void Start()
    {
        start = _rect.anchoredPosition;
        end += start;
    }

    private void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;

        _rect.anchoredPosition = Vector2.Lerp(start, end, t);
    }
}
