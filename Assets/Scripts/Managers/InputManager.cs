using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class InputManager : MonoBehaviour
{
    private PlayerInputManager _manager;
    [SerializeField] private GameObject _playerPrefab;
    private const int MAX_PLAYERS = 4;
    private InputDevice[] _devicesConnected = new InputDevice[MAX_PLAYERS];
    private string[] _controlSchemes = new string[MAX_PLAYERS];
    private bool _wasdConnected, _arrowsConnected;

    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        _manager = GetComponent<PlayerInputManager>();
    }

    void Update()
    {
        if (!PlayersAvailable() || !_manager.joiningEnabled) return;
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && !_wasdConnected)
        {
            AddPlayer("WASD", Keyboard.current);
            _wasdConnected = true;
        }
        if (Keyboard.current.rightCtrlKey.wasPressedThisFrame && !_arrowsConnected)
        {
            AddPlayer("Arrows", Keyboard.current);
            _arrowsConnected = true;
        }
        foreach (Gamepad gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                bool lFoundController = false;
                foreach (InputDevice device in _devicesConnected)
                {
                    if (device == gamepad)
                    {
                        lFoundController = true;
                        break;
                    }
                }
                if (!lFoundController) AddPlayer("Gamepad", gamepad);
            }
        }
    }

    private void AddPlayer(string pControlScheme, InputDevice pDeviceToPair)
    {
        int playerID = _manager.playerCount;
        _devicesConnected[playerID] = pDeviceToPair;
        _controlSchemes[playerID] = pControlScheme;
        PlayerManager.Instance.OnJoin(playerID);

        SpawnPlayer(playerID);
    }

    private void SpawnPlayer(int pPlayerID)
    {
        PlayerInput lNewPlayer = PlayerInput.Instantiate(_playerPrefab, controlScheme: _controlSchemes[pPlayerID], pairWithDevice: _devicesConnected[pPlayerID]);
        Fish lFish = lNewPlayer.GetComponent<Fish>();
        lFish.SetFishIndex(pPlayerID);
        InputMode lDeviceType = new();
        if (_devicesConnected[pPlayerID] is Keyboard) lDeviceType = InputMode.Keyboard;
        else if (_devicesConnected[pPlayerID] is Gamepad) lDeviceType = InputMode.Gamepad;
        PlayerManager.Instance.AddPlayerCharacter(lNewPlayer.transform, pPlayerID, lDeviceType);
    }

    public void RespawnPlayers()
    {
        for (int playerID = 0; playerID < _devicesConnected.Length; playerID++)
        {
            if (_devicesConnected[playerID] == null) continue;
            SpawnPlayer(playerID);
        }
    }

    private bool PlayersAvailable() => _manager.playerCount < _manager.maxPlayerCount;

    public void EnableDeviceConnection(bool pEnable)
    {
        if (pEnable) _manager.EnableJoining(); 
        else _manager.DisableJoining();        
    }

    public void DisconnectAllDevices()
    {
        PlayerManager.Instance.DeletePlayers();
        _wasdConnected = false;
        _arrowsConnected = false;
        _devicesConnected = new InputDevice[MAX_PLAYERS];
        _controlSchemes = new string[MAX_PLAYERS];
    }
}
