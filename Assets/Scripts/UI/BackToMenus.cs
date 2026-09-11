using DG.Tweening;
using UnityEngine;

public class BackToMenus : MonoBehaviour
{
    [SerializeField] private BigWaveTransition _transition;
    [SerializeField] private CanvasGroup _menu;

    [SerializeField] private GameObject _title;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _menuButtons;
    [SerializeField] private GameObject _controllersSelection;

    [SerializeField] private float _duration = 1f;

    public void DoBackToMenus()
    {
        _transition.EndTransition();

        _title.SetActive(true);
        _background.SetActive(true);
        _menuButtons.SetActive(false);
        _controllersSelection.SetActive(false);

        _menu.DOFade(0f, _duration).OnComplete(OnTransitionComplete);
    }

    private void OnTransitionComplete()
    {
        _menu.alpha = 1f;
        _menu.gameObject.SetActive(false);
    }
}
