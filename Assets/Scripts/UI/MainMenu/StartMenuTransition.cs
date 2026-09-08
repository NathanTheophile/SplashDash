using UnityEngine;
using DG.Tweening;

public class StartMenuTransition : MonoBehaviour
{
    [SerializeField] private GameObject _nextMenu;
    [SerializeField] private GameObject _deactivated;

    [Header("Title")]
    [SerializeField] private RectTransform _titleRect;
    [SerializeField] private RectTransform _targetRect;
    [SerializeField] private float _Duration = 0.5f;

    public void Transition()
    {
        if (!gameObject.activeSelf) return;
        _deactivated.SetActive(false);

        _titleRect.DOAnchorPos(_targetRect.anchoredPosition, _Duration).SetEase(Ease.OutSine);
        _titleRect.DOSizeDelta(_targetRect.sizeDelta, _Duration);
        _nextMenu.SetActive(true);
    }
}
