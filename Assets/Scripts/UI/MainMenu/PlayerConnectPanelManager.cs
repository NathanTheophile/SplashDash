using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerConnectPanelManager : MonoBehaviour
{
    [SerializeField] private PlayerConnectPanel[] _panels;
    [SerializeField] private TextMeshProUGUI _startText;
    [SerializeField] private Button _startButton;

    private PlayerManager _manager;
    private bool _selectionPending;

    public static PlayerConnectPanelManager Instance { get; private set; }

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
        _manager = PlayerManager.Instance;

        UpdateText();
    }

    public void ResetAll()
    {
        foreach (var panel in _panels)
        {
            panel.RemovePanel();
        }
    }

    public void UpdateText()
    {
        if(_manager == null) return;

        int playerNbr = _manager.PlayerNumber;

        if (playerNbr > 1)
        {
            _startText.gameObject.SetActive(false);
            _startButton.interactable = true;
            SelectStartButtonWithController();
            return;
        }

        _startButton.interactable = false;
        _startText.gameObject.SetActive(true);
        _startText.text = $"Need {2 - playerNbr} More Players to Start the Game";
    }

    private void SelectStartButtonWithController()
    {
        if (_selectionPending || EventSystem.current == null)
            return;

        _selectionPending = true;
        StartCoroutine(SelectStartButtonNextFrame());
    }

    private IEnumerator SelectStartButtonNextFrame()
    {
        yield return null;

        _selectionPending = false;
        EventSystem.current?.SetSelectedGameObject(_startButton.gameObject);
    }

    public void EditPanel(int id, InputMode usedInput)
    {
        if (id >= _panels.Length) return;

        _panels[id].SetPanel(id, usedInput);
    }

    public void RemovePanel(int id)
    {
        if (id >= _panels.Length) return;

        _panels[id].RemovePanel();
    }
}
