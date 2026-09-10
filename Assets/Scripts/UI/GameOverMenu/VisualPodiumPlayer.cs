using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VisualPodiumPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private Image _image;

    [SerializeField] private PlayersSpritesColorsSO _skinMixer;

    public void SetIdTime(int id, int time)
    {
        gameObject.SetActive(true);
        _time.text = $"{time} s";
        _name.text = $"Player {id}";

        bool isEmpty = _skinMixer.data.Length == 0;
        if (isEmpty) return;

        var player = PlayerManager.Instance.GetPlayerByID(id);

        var mixer = _skinMixer.data[player.SkinID];
        _image.sprite = mixer.sprites;
    }
}
