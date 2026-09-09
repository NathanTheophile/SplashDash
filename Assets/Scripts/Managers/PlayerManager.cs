using System.Collections.Generic;
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

    private readonly List<PlayerData> _players = new List<PlayerData>();
    
    public void OnJoin(int id)
    {
        int skinID = Random.Range(0, _colorMixer.data.Length);
        var player = new PlayerData(id, skinID);

        _players.Add(player);
    }
}
