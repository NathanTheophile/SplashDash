using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class InputManager : MonoBehaviour
{
    private PlayerInputManager _manager;
    [SerializeField] private GameObject _playerPrefab;
    private const int MAX_PLAYERS = 4;
    private InputDevice[] _devicesConnected = new InputDevice[MAX_PLAYERS];
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
        if (!PlayersAvailable()) return;
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
        _devicesConnected[_manager.playerCount] = pDeviceToPair;
        PlayerManager.Instance.OnJoin(_manager.playerCount);

        PlayerInput lNewPlayer = PlayerInput.Instantiate(_playerPrefab, controlScheme: pControlScheme, pairWithDevice: pDeviceToPair);
        Fish lFish = lNewPlayer.GetComponent<Fish>();
        lFish.SetFishIndex(_manager.playerCount - 1);


        PlayerManager.Instance.AddPlayerCharacter(lNewPlayer.transform);
    }

    private bool PlayersAvailable() => _manager.playerCount < _manager.maxPlayerCount;
}
