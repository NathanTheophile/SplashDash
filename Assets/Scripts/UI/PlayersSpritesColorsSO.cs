using System;
using UnityEngine;

[Serializable]
public struct FishSkin
{
    public Sprite fishFin;
    public Sprite fishBody;
}

[Serializable]
public struct SkinBGMixer
{
    public Color mainBGColor;
    public Color lightBGColor;
    public Sprite sprites;
    public Sprite knockedSprite;
    public FishSkin fishSkin;
}

[CreateAssetMenu(fileName = "PlayersSpritesColorsSO", menuName = "Scriptable Objects/PlayersSpritesColorsSO")]
public class PlayersSpritesColorsSO : ScriptableObject
{
    public SkinBGMixer[] data = new SkinBGMixer[0];
}
