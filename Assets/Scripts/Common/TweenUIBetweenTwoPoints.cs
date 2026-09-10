using DG.Tweening;
using UnityEngine;

public class TweenUIBetweenTwoPoints : MonoBehaviour
{
    private RectTransform _object;
    [SerializeField] private RectTransform _start;
    [SerializeField] private RectTransform _end;

    [SerializeField] private Ease _easeFrom;
    [SerializeField] private Ease _easeTo;

    private void Awake()
    {
        _object = gameObject.GetComponent<RectTransform>();
    }

    public Tween DoTweenTo(float duration)
    {
        _object.anchoredPosition = _start.anchoredPosition;
        return _object.DOAnchorPos(_end.anchoredPosition, duration).SetEase(_easeTo);
    }

    public Tween DoTweenFrom(float duration)
    {
        _object.anchoredPosition = _end.anchoredPosition;
        return _object.DOAnchorPos(_start.anchoredPosition, duration).SetEase(_easeFrom);
    }
}
