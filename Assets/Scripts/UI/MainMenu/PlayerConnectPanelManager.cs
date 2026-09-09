using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConnectPanelManager : MonoBehaviour
{
    [SerializeField] private PlayerConnectPanel[] _panels;
    [SerializeField] private TextMeshProUGUI _startText;
    [SerializeField] private Button _startButton;

    private PlayerManager _manager;

    private void Start()
    {
        _manager = PlayerManager.Instance;

        UpdateText();
    }

    public void UpdateText()
    {
        if(_manager == null) return;

        int playerNbr = _manager.PlayerNumber;

        if (playerNbr > 1)
        {
            _startText.gameObject.SetActive(false);
            _startButton.interactable = true;
            return;
        }

        _startButton.interactable = false;
        _startText.gameObject.SetActive(true);
        _startText.text = $"Need {2 - playerNbr} More Players to Start the Game";
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