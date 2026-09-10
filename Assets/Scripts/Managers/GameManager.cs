using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HUD _hud;
    private PlayerManager _playermanager;

    private float _elapsedTime = 0f;
    private int _time = 0;
    private bool _clockRunning = false;
    

    private void Start()
    {
        _playermanager = PlayerManager.Instance;

        GameScoresData gameScoresData = new GameScoresData()
        {
            orderedScores = new PlayerScore[4]
        };

        Fish.OnPlayerDeath += OnPlayerDeath;
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
        int playerSkin = _playermanager.GetPlayerByID(idPlayer).SkinID;
        if(idKiller == -1)
        {
            _hud.KillSetter.SetVisual(playerSkin);
        }

        int killerSkin = _playermanager.GetPlayerByID(idKiller).SkinID;
        _hud.KillSetter.SetVisual(killerSkin, playerSkin);
    }
}
