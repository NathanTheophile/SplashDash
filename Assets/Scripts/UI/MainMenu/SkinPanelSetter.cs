using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinPanelSetter : MonoBehaviour
{
    [SerializeField] private Image _bgImage;
    [SerializeField] private Image _bg;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;

    [SerializeField] private PlayersSpritesColorsSO _skinMixer;

    public void SetIdImages(int id)
    {
        bool isEmpty = _skinMixer.data.Length == 0;
        if (isEmpty) return;

        var mixer = _skinMixer.data[id];

        _bgImage.color = mixer.lightBGColor;
        _bg.color = mixer.mainBGColor;
        _text.color = mixer.mainBGColor;
        _image.sprite = mixer.sprites;
    }
}
