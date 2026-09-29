using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StartMenuTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup _buttonsCanvasGroup;
    [SerializeField] private GameObject _nextMenu;
    [SerializeField] private GameObject _deactivated;
    [SerializeField] private AnyButtonInput _buttonInput;

    [Header("Title")]
    [SerializeField] private RectTransform _titleRect;
    [SerializeField] private RectTransform _targetRect;
    [SerializeField] private RectTransform _startRect;
    [SerializeField] private float _Duration = 0.5f;

    [Header("Fishes")]
    [SerializeField] private Image _fishes;
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _endColor;

    [Header("Bouee")]
    [SerializeField] private TweenUIBetweenTwoPoints _tweenersBouee1;
    [SerializeField] private TweenUIBetweenTwoPoints _tweenersBouee2;
    [SerializeField] private CanvasGroup _boueeShadows;

    [SerializeField] private Button _clickButton;

    private void Start()
    {
        //_buttonInput.OnButtonEvent.AddListener(DoTransition);
        _clickButton.onClick.AddListener(DoTransition);
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
        _nextMenu.SetActive(true);

        _titleRect.DOAnchorPos(_targetRect.anchoredPosition, _Duration).SetEase(Ease.OutSine);
        _titleRect.DOSizeDelta(_targetRect.sizeDelta, _Duration).SetEase(Ease.OutSine);

        _buttonsCanvasGroup.alpha = 0f;
        _buttonsCanvasGroup.DOFade(1, _Duration + 0.5f).OnComplete(OnToTweenComplete);

        _fishes.DOColor(_endColor, _Duration);


        _boueeShadows.alpha = 0f;


        var seq = DOTween.Sequence();
        seq.Append(_tweenersBouee2.DoTweenTo(_Duration));
        seq.Append(_boueeShadows.DOFade(1, 0.5f));
        _tweenersBouee1.DoTweenTo(_Duration + 0.3f);
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

        _buttonsCanvasGroup.DOFade(0f, _Duration).OnComplete(OnFromTweenComplete);

        _fishes.DOColor(_startColor, _Duration);

        _boueeShadows.alpha = 1f;
        _boueeShadows.DOFade(0, 0.2f);

        _tweenersBouee1.DoTweenFrom(_Duration/2);
        _tweenersBouee2.DoTweenFrom(_Duration/2);
    }

    private void OnFromTweenComplete()
    {
        _nextMenu.SetActive(false);
        TransitionManager.IsPlaying = false;
    }

    private void OnDestroy()
    {
        //_buttonInput.OnButtonEvent.RemoveListener(DoTransition);

        _clickButton.onClick.RemoveListener(DoTransition);
    }
}
