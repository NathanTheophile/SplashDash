using UnityEngine;

public class FishAnimator : MonoBehaviour
{
    [SerializeField] private GameObject _stunSprite;

    [SerializeField] private FinAnimator _finLeft;
    [SerializeField] private FinAnimator _finRight;

    [SerializeField] private SpriteRenderer _body;

    [SerializeField] private PlayersSpritesColorsSO _spritesColors;

    public void SetStunned()
    {
        _stunSprite.SetActive(true);
    }
    public void RemoveStun()
    {
        _stunSprite.SetActive(false);
    }

    public void SetSpeed(float speed, float multiplier)
    {
        float totalSpeed = speed * multiplier;

        _finLeft.SetSpeed(totalSpeed);
        _finRight.SetSpeed(totalSpeed);
    }

    public void SetSkin(int playerID)
    {
        var player = PlayerManager.Instance.GetPlayerByID(playerID);

        var skin = _spritesColors.data[player.SkinID].fishSkin;

        _body.sprite = skin.fishBody;
        _finLeft.SetSkin(skin.fishBody);
        _finRight.SetSkin(skin.fishBody);
    }
}
