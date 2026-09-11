using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    public static bool IsPlaying = false;

    [SerializeField] private BigWaveTransition _transition;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private GameObject _mainMenuCamera;
    [SerializeField] private GameObject _bg;
    [SerializeField] private GameObject _currentMenu;
    [SerializeField] private GameObject _title;
    

    private enum nextState
    {
        empty,
        gameOver,
        gameplay,

    }

    private nextState state = nextState.empty;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _transition.OnAnimationHalfed.AddListener(OnHalf);
        _transition.OnCanvasFade.AddListener(OnFade);
    }

    public void GoToGameOver(GameScoresData pScores)
    {
        _gameOverScreen.GetComponentInChildren<VisualPodiumSetter>(true).SetGameScores(pScores);
        state = nextState.gameOver;
        _transition.DoTransition();
    }

    public void GoToGameplay()
    {
        state = nextState.gameplay;
        _transition.DoTransition();
    }

    public void ResetToMenu()
    {
        state = nextState.empty;
        _gameOverScreen.SetActive(false);
    }

    private void OnHalf()
    {
        switch (state)
        {
            case nextState.gameplay:

                _mainMenuCamera.SetActive(false);
                _bg.gameObject.SetActive(false);
                _currentMenu.gameObject.SetActive(false);
                _gameOverScreen.SetActive(false);

                _gameManager.StartGame();
                _transition.EndTransition();

                //_transition.PrepareGameplay();
                return;
        }
    }

    private void OnFade()
    {
        switch (state)
        {
            case nextState.gameOver:
                _gameOverScreen.SetActive(true);

                return;
        }
    }

    private void OnDestroy()
    {
        _transition.OnAnimationHalfed.RemoveListener(OnHalf);
        _transition.OnCanvasFade.RemoveListener(OnFade);
    }
}
