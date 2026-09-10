using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum InputMode
{
    Keyboard,
    Gamepad
}

public class PlayerConnectPanel : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _usedInputImage;
    [SerializeField] private SkinPanelSetter _skinMixer;

    [Header("Sprites")]
    [SerializeField] private Sprite _keyboardSprite;
    [SerializeField] private Sprite _controllerSprite;


    public void SetPanel(int id, InputMode usedInput)
    {
        _skinMixer.gameObject.SetActive(true);
        _text.text = $"Player {id + 1}";

        var currentsprite = usedInput == 0 ? _keyboardSprite : _controllerSprite;
        _usedInputImage.sprite = currentsprite;

        var player = PlayerManager.Instance.GetPlayerByID(id);

        _skinMixer.SetIdImages(player.SkinID);
    }

    public void RemovePanel()
    {
        _skinMixer.gameObject.SetActive(false);
    }
}
