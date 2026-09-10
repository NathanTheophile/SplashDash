using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ControllerSelectionTransition : MonoBehaviour
{
    [SerializeField] private TweenUIBetweenTwoPoints _title;
    [SerializeField] private TweenUIBetweenTwoPoints _currentMenu;
    [SerializeField] private TweenUIBetweenTwoPoints _mainButtons;

    [SerializeField] private CanvasGroup _bg;
    [SerializeField] private CanvasGroup _boueeShadows;

    [SerializeField] private float _Duration = 0.5f;


    public void DoTransition()
    {
        TransitionTo();
    }

    public void TransitionTo()
    {
        if (TransitionManager.IsPlaying) return;
        TransitionManager.IsPlaying = true;

        _currentMenu.gameObject.SetActive(true);

        _bg.alpha = 0f;
        _bg.DOFade(1f, _Duration).OnComplete(OnToTweenComplete);

        _boueeShadows.alpha = 1f;
        _boueeShadows.DOFade(0f, 0.2f);

        _title.DoTweenTo(_Duration);
        _currentMenu.DoTweenTo(_Duration);
        _mainButtons.DoTweenTo(_Duration);

        InputManager.Instance.EnableDeviceConnection(true);
    }

    private void OnToTweenComplete()
    {
        TransitionManager.IsPlaying = false;
        _mainButtons.gameObject.SetActive(false);
    }

    public void TransitionFrom()
    {
        if (TransitionManager.IsPlaying) return;
        TransitionManager.IsPlaying = true;

        _mainButtons.gameObject.SetActive(true);

        _bg.alpha = 1f;
        _bg.DOFade(0f, _Duration).OnComplete(OnFromTweenComplete);

        _boueeShadows.alpha = 0f;
        _boueeShadows.DOFade(1f, 0.2f);

        _title.DoTweenFrom(_Duration);
        _currentMenu.DoTweenFrom(_Duration);
        _mainButtons.DoTweenFrom(_Duration);

        InputManager.Instance.EnableDeviceConnection(false);
        InputManager.Instance.DisconnectAllDevices();
    }

    private void OnFromTweenComplete()
    {
        TransitionManager.IsPlaying = false;
        _currentMenu.gameObject.SetActive(false);
    }
}
