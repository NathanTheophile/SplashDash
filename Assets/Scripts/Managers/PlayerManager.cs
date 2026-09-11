using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public List<int> colorindexes = new ();
    private Transform _playerContainer;

    public static PlayerManager Instance;

    public void Start()
    {
        Instance = this;
    }

    public void SetupGameplay(Scene pGameplayScene)
    {
        foreach (GameObject gameObject in pGameplayScene.GetRootGameObjects())
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
        
        int skinID = 0;

        bool lvalid = true;
        while (lvalid)
        {
            lvalid = false;
            skinID = Random.Range(0, _colorMixer.data.Length);
            foreach(var ind in colorindexes)
            {
                if (skinID == ind) lvalid = true;
            }


        }

        colorindexes.Add(skinID);

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
        pPlayerTransform.GetComponent<Fish>().SetGameplayActive(false);

        if (PlayerConnectPanelManager.Instance != null)
        {
            PlayerConnectPanelManager.Instance.UpdateText();
            PlayerConnectPanelManager.Instance.EditPanel(pId, pDeviceType);
            return;
        }

        pPlayerTransform.GetComponent<Fish>().SetGameplayActive(true);
    }

    public void ActivatePlayers()
    {
        for (int i = 0; i < _playerContainer.childCount; i++)
        {
            Transform playerObject = _playerContainer.GetChild(i);
            playerObject.gameObject.SetActive(true);
            playerObject.GetComponent<Fish>().SetGameplayActive(true);
        }
    }

    public void SetPlayerGameplayActive(int pPlayerID, bool pActive)
    {
        for (int i = 0; i < _playerContainer.childCount; i++)
        {
            Fish fish = _playerContainer.GetChild(i).GetComponent<Fish>();
            if (fish.FishIndex != pPlayerID) continue;

            fish.SetGameplayActive(pActive);
            return;
        }
    }

    public void DeactivatePlayers()
    {
        for (int i = 0; i < _playerContainer.childCount; i++)
            _playerContainer.GetChild(i).GetComponent<Fish>().SetGameplayActive(false);
    }

    public void DeletePlayers()
    {
        int lPlayerCount = _playerContainer.childCount;
        for (int i = lPlayerCount - 1; i >= 0; i--)
        {
            Destroy(_playerContainer.GetChild(i).gameObject);
        }
        _players = new PlayerData[4];
        colorindexes.Clear();
        PlayerNumber = _players.Count(d => d != null);
    }
}
