using DG.Tweening;
using UnityEngine;

public class ControllerSelectionTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup _currentMenuCanvasGroup;
    [SerializeField] private CanvasGroup _mainButtonsCanvasGroup;
    [SerializeField] private CanvasGroup _titleCanvasGroup;

    [SerializeField] private float _Duration = 0.5f;


    public void DoTransition()
    {
        TransitionTo();
    }

    public void TransitionTo()
    {
        if (TransitionManager.IsPlaying) return;
        TransitionManager.IsPlaying = true;

        _currentMenuCanvasGroup.gameObject.SetActive(true);
        _currentMenuCanvasGroup.alpha = 0f;

        _currentMenuCanvasGroup.DOFade(1f, _Duration).OnComplete(OnToTweenComplete);
        _mainButtonsCanvasGroup.DOFade(0f, _Duration);
        _titleCanvasGroup.DOFade(0f, _Duration);
    }

    private void OnToTweenComplete()
    {
        TransitionManager.IsPlaying = false;
        _mainButtonsCanvasGroup.gameObject.SetActive(false);
    }

    public void TransitionFrom()
    {
        if (TransitionManager.IsPlaying) return;
        TransitionManager.IsPlaying = true;

        _mainButtonsCanvasGroup.gameObject.SetActive(true);
        _mainButtonsCanvasGroup.alpha = 0f;
        _titleCanvasGroup.alpha = 0f;

        _currentMenuCanvasGroup.DOFade(0f, _Duration).OnComplete(OnFromTweenComplete);
        _mainButtonsCanvasGroup.DOFade(1f, _Duration);
        _titleCanvasGroup.DOFade(1f, _Duration);
    }

    private void OnFromTweenComplete()
    {
        TransitionManager.IsPlaying = false;
        _currentMenuCanvasGroup.gameObject.SetActive(false);
    }
}
