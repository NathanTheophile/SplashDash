using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenus : MonoBehaviour
{
    [SerializeField] private BigWaveTransition _transition;
    [SerializeField] private CanvasGroup _menu;

    [SerializeField] private GameObject _title;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _menuButtons;
    [SerializeField] private GameObject _controllersSelection;
    [SerializeField] private Camera _mainMenuCamera;
    [SerializeField] private StartMenuTransition _startMenuTransition;

    [SerializeField] private float _duration = 1f;

    public void DoBackToMenus()
    {
        InputManager.Instance.EnableDeviceConnection(false);
        InputManager.Instance.DisconnectAllDevices();
        FindFirstObjectByType<TrailManager>()?.ClearTrails();
        GameManager.Instance.ResetManager();

        Scene gameplayScene = SceneManager.GetSceneByName("Gameplay");
        if (gameplayScene.isLoaded)
            SceneManager.UnloadSceneAsync(gameplayScene);

        _mainMenuCamera.enabled = true;
        _transition.EndTransition();

        _title.SetActive(true);
        _background.SetActive(true);
        _menuButtons.SetActive(false);
        _controllersSelection.SetActive(false);
        _startMenuTransition.TransitionFrom();

        _menu.DOFade(0f, _duration).OnComplete(OnTransitionComplete);
    }

    private void OnTransitionComplete()
    {
        _menu.alpha = 1f;
        _menu.gameObject.SetActive(false);
        TransitionManager.Instance.ResetToMenu();
    }
}
