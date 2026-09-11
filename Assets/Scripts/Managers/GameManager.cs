using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HUD _hud;
    private PlayerManager _playermanager;

    private float _elapsedTime = 0f;
    private int _time = 0;
    private bool _clockRunning = false;
    private readonly HashSet<int> _eliminatedPlayers = new();
    private readonly List<PlayerScore> _orderedScores = new();
    private Coroutine _gameOverRoutine;

    public static GameManager Instance { get; private set; }
    public static event System.Action GameStarted;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _playermanager = PlayerManager.Instance;
        Fish.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        Fish.OnPlayerDeath -= OnPlayerDeath;
    }

    public void StartGame()
    {
        ResetManager();
        PlayerManager.Instance.ActivatePlayers();
        InputManager.Instance.EnableDeviceConnection(false);
        StartClock();
        GameStarted?.Invoke();
    }

    public void SetupGameplay(Scene pGameplayScene)
    {
        foreach (GameObject rootObject in pGameplayScene.GetRootGameObjects())
        {
            HUD hud = rootObject.GetComponentInChildren<HUD>(true);
            if (hud == null) continue;

            _hud = hud;
            return;
        }
    }

    public void ResetManager()
    {
        _elapsedTime = 0f;
        _time = 0;
        _clockRunning = false;
        _eliminatedPlayers.Clear();
        _orderedScores.Clear();
        _gameOverRoutine = null;
    }

    public void StartClock()
    {
        _clockRunning = true;
    }

    public void StopClock()
    {
        _clockRunning = false;
    }

    private void FixedUpdate()
    {
        if (!_clockRunning) return;

        _elapsedTime += Time.fixedDeltaTime;
        if (_elapsedTime > _time)
        {
            _time = Mathf.CeilToInt(_elapsedTime);
            _hud.SetTime(_time);
        }
    }

    public void OnPlayerDeath(int idPlayer, int idKiller)
    {
        if (_playermanager == null)
            _playermanager = PlayerManager.Instance;

        PlayerData killedPlayer = _playermanager.GetPlayerByID(idPlayer);
        if (killedPlayer == null || !_eliminatedPlayers.Add(idPlayer))
            return;

        int playerSkin = killedPlayer.SkinID;
        _orderedScores.Insert(0, new PlayerScore { id = idPlayer, time = _time });
        _playermanager.SetPlayerGameplayActive(idPlayer, false);

        if (idKiller == -1)
        {
            _hud.KillSetter.SetVisual(playerSkin);
        }
        else
        {
            PlayerData killer = _playermanager.GetPlayerByID(idKiller);
            if (killer != null)
                _hud.KillSetter.SetVisual(killer.SkinID, playerSkin);
        }

        if (_gameOverRoutine == null)
            _gameOverRoutine = StartCoroutine(CheckGameOverAtEndOfFrame());
    }

    private IEnumerator CheckGameOverAtEndOfFrame()
    {
        yield return null;
        _gameOverRoutine = null;

        int remainingPlayers = _playermanager.PlayerNumber - _eliminatedPlayers.Count;
        if (remainingPlayers > 1) yield break;

        AddWinnerToScores();
        StopClock();
        _playermanager.DeactivatePlayers();

        GameScoresData scores = new GameScoresData
        {
            orderedScores = _orderedScores.ToArray()
        };

        TransitionManager.Instance.GoToGameOver(scores);
    }

    private void AddWinnerToScores()
    {
        for (int playerID = 0; playerID < 4; playerID++)
        {
            if (_playermanager.GetPlayerByID(playerID) == null) continue;
            if (_eliminatedPlayers.Contains(playerID)) continue;

            _orderedScores.Insert(0, new PlayerScore { id = playerID, time = _time });
            return;
        }
    }
}
