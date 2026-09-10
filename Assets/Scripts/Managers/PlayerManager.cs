using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData
{
    public int PlayerID;
    public int SkinID;

    public PlayerData(int playerID, int skinID)
    {
        PlayerID = playerID;
        SkinID = skinID;
    }
}

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayersSpritesColorsSO _colorMixer;

    private readonly PlayerData[] _players = new PlayerData[4];
    public int PlayerNumber {  get; private set; }
    public static PlayerManager Instance;

    public void Start()
    {
        Instance = this;
    }

    //IDS : 0 1 2 3
    public void OnJoin(int id)
    {
        if (0 > id || id > 3) return;
        int skinID = Random.Range(0, _colorMixer.data.Length);
        var player = new PlayerData(id, skinID);

        _players[id] = player;

        PlayerNumber = _players.Count(d => d != null);
    }

    public void OnQuit(int id)
    {
        if (0 > id || id > 3) return;

        _players[id] = null;


        PlayerNumber = _players.Count(d => d != null);
    }

    public PlayerData GetPlayerByID(int id)
    {
        if (0 > id || id > 3) return null;
        return _players[id];
    }
}
