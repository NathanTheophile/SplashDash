using System;
using UnityEngine;

[Serializable]
public struct SkinBGMixer
{
    public Color mainBGColor;
    public Color lightBGColor;
    public Sprite sprites;
}

[CreateAssetMenu(fileName = "PlayersSpritesColorsSO", menuName = "Scriptable Objects/PlayersSpritesColorsSO")]
public class PlayersSpritesColorsSO : ScriptableObject
{
    public SkinBGMixer[] data = new SkinBGMixer[0];
}
