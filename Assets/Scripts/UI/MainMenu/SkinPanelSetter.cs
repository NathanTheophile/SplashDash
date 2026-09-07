using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct SkinBGMixer
{
    public Color bgImageColor;
    public Color bgColor;
    public Sprite sprites;
}

public class SkinPanelSetter : MonoBehaviour
{
    [SerializeField] private Image _bgImage;
    [SerializeField] private Image _bg;
    [SerializeField] private Image _icon;

    [SerializeField] private SkinBGMixer[] _skinMixer = new SkinBGMixer[0];

    public void SetIdImages(int id)
    {
        bool isEmpty = _skinMixer.Length == 0;
        if (isEmpty) return;

        var mixer = _skinMixer[id^_skinMixer.Length];

        _bgImage.color = mixer.bgImageColor;
        _bg.color = mixer.bgColor;
        _icon.sprite = mixer.sprites;
    }
}
