using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StartMenuTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup _nextMenuCanvasGroup;
    [SerializeField] private GameObject _nextMenu;
    [SerializeField] private GameObject _deactivated;
    [SerializeField] private AnyButtonInput _buttonInput;

    [Header("Title")]
    [SerializeField] private RectTransform _titleRect;
    [SerializeField] private RectTransform _targetRect;
    [SerializeField] private RectTransform _startRect;
    [SerializeField] private float _Duration = 0.5f;

    [Header("Colors")]
    [SerializeField] private Image _fishes;
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _endColor;

    private void Start()
    {
        _buttonInput.OnButtonEvent.AddListener(DoTransition);
    }

    public void DoTransition()
    {
        TransitionTo();
    }

    public void TransitionTo()
    {
        if (TransitionManager.IsPlaying) return;
        TransitionManager.IsPlaying = true;

        _deactivated.SetActive(false);

        _titleRect.DOAnchorPos(_targetRect.anchoredPosition, _Duration).SetEase(Ease.OutSine);
        _titleRect.DOSizeDelta(_targetRect.sizeDelta, _Duration).SetEase(Ease.OutSine);

        _nextMenuCanvasGroup.alpha = 0f;
        _nextMenuCanvasGroup.DOFade(1, _Duration).OnComplete(OnToTweenComplete);

        _fishes.DOColor(_endColor, _Duration);

        _nextMenu.SetActive(true);
    }

    private void OnToTweenComplete()
    {
        TransitionManager.IsPlaying = false;
        _buttonInput.enabled = false;
    }

    public void TransitionFrom()
    {
        if (TransitionManager.IsPlaying) return;
        TransitionManager.IsPlaying = true;

        _deactivated.SetActive(true);

        _titleRect.DOAnchorPos(_startRect.anchoredPosition, _Duration).SetEase(Ease.OutSine);
        _titleRect.DOSizeDelta(_startRect.sizeDelta, _Duration).SetEase(Ease.OutSine);

        _nextMenuCanvasGroup.DOFade(0f, _Duration).OnComplete(OnFromTweenComplete);

        _fishes.DOColor(_startColor, _Duration);
    }

    private void OnFromTweenComplete()
    {
        _nextMenu.SetActive(false);
        TransitionManager.IsPlaying = false;
    }

    private void OnDestroy()
    {
        _buttonInput.OnButtonEvent.RemoveListener(DoTransition);
    }
}
