using System;
using UnityEngine;
using UnityEngine.UI;

public class SkinPanelSetter : MonoBehaviour
{
    [SerializeField] private Image _bgImage;
    [SerializeField] private Image _bg;
    [SerializeField] private Image _image;

    [SerializeField] private PlayersSpritesColorsSO _skinMixer;

    public void SetIdImages(int id)
    {
        bool isEmpty = _skinMixer.data.Length == 0;
        if (isEmpty) return;

        var mixer = _skinMixer.data[id^_skinMixer.data.Length];

        _bgImage.color = mixer.mainBGColor;
        _bg.color = mixer.lightBGColor;
        _image.sprite = mixer.sprites;
    }
}
