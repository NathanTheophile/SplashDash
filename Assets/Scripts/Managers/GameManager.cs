using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HUD _hud;
    private PlayerManager _playermanager;

    private float _elapsedTime = 0f;
    private int _time = 0;
    private bool _clockRunning = false;

    [SerializeField] private Transform[] _levelPrefabs;

    public static GameManager Instance { get; private set; }

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

        GameScoresData gameScoresData = new GameScoresData()
        {
            orderedScores = new PlayerScore[4]
        };

        Fish.OnPlayerDeath += OnPlayerDeath;
    }

    public void StartGame()
    {
        Instantiate(GetRandomLevel(), SceneManager.GetSceneByName("Gameplay"));
    }

    private Transform GetRandomLevel()
    {
        return _levelPrefabs[Random.Range(0, _levelPrefabs.Length - 1)];
    }

    public void ResetManager()
    {
        _elapsedTime = 0f;
        _time = 0;
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
    if (killedPlayer == null)
        return;

    int playerSkin = killedPlayer.SkinID;

    if (idKiller == -1)
    {
        _hud.KillSetter.SetVisual(playerSkin);
        return;
    }

    PlayerData killer = _playermanager.GetPlayerByID(idKiller);
    if (killer == null)
        return;

    _hud.KillSetter.SetVisual(killer.SkinID, playerSkin);
}
}
