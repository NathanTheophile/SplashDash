using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private PlayerData[] _players = new PlayerData[4];
    public int PlayerNumber {  get; private set; }
    private Transform _playerContainer;

    public static PlayerManager Instance;

    public void Start()
    {
        Instance = this;

        Scene lGameplayScene = SceneManager.GetSceneByName("Gameplay");
        foreach (GameObject gameObject in lGameplayScene.GetRootGameObjects())
        {
            if (gameObject.name == "PlayerContainer")
            {
                _playerContainer = gameObject.transform;
                break;
            }
        }
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

    public void AddPlayerCharacter(Transform pPlayerTransform, int pId, InputMode pDeviceType)
    {
        pPlayerTransform.SetParent(_playerContainer);
        //pPlayerTransform.gameObject.SetActive(false);

        PlayerConnectPanelManager.Instance.UpdateText();
        PlayerConnectPanelManager.Instance.EditPanel(pId, pDeviceType);
    }

    public void ActivatePlayers()
    {
        foreach (Transform playerObject in _playerContainer.GetComponentsInChildren<Transform>())
        {
            playerObject.gameObject.SetActive(true);
        }
    }

    public void DeletePlayers()
    {
        int lPlayerCount = _playerContainer.childCount;
        for (int i = lPlayerCount - 1; i >= 0; i--)
        {
            Destroy(_playerContainer.GetChild(i).gameObject);
        }
        _players = new PlayerData[4];
        PlayerNumber = _players.Count(d => d != null);
    }
}
