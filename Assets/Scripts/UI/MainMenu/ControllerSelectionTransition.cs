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
    [SerializeField] private BigWaveTransition _sceneLoader;
    [SerializeField] private Camera _mainMenuCamera;

    [SerializeField] private PlayerConnectPanelManager _playerConnectPanelManager;

    [SerializeField] private float _Duration = 0.5f;

    private void OnEnable()
    {
        GameManager.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameManager.GameStarted -= OnGameStarted;

        _currentMenu.ResetTween();
        _title.ResetTween();
        _mainButtons.ResetTween();
    }

    public void OnGameStarted()
    {
        //TransitionManager.Instance.GoToGameplay();
    }

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

        _playerConnectPanelManager.ResetAll();
        _sceneLoader.PrepareGameplay();
    }


    private void OnToTweenComplete()
    {
        TransitionManager.IsPlaying = false;
        _title.gameObject.SetActive(false);
        _mainButtons.gameObject.SetActive(false);
    }

    public void TransitionFrom()
    {
        if (TransitionManager.IsPlaying) return;
        TransitionManager.IsPlaying = true;

        _mainButtons.gameObject.SetActive(true);
        _title.gameObject.SetActive(true);

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
        _title.gameObject.SetActive(true);
    }
}
