using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    public static bool IsPlaying = false;

    [SerializeField] private BigWaveTransition _transition;
    [SerializeField] private GameObject _gameOverScreen;

    private enum nextState
    {
        empty,
        gameOver,

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
        _transition.DoTransition();
        state = nextState.gameOver;
    }

    private void OnHalf()
    {
        switch (state)
        {
            case nextState.empty:

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
}
