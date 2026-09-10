using UnityEngine;

public class FishAnimator : MonoBehaviour
{
    [SerializeField] private GameObject _stunSprite;

    [SerializeField] private FinAnimator _finLeft;
    [SerializeField] private FinAnimator _finRight;

    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private SpriteShaker _shaker;

    [SerializeField] private PlayersSpritesColorsSO _spritesColors;
    [SerializeField] private ParticleSystem.EmissionModule _emissionModule;

    public void SetStunned()
    {
        _stunSprite.SetActive(true);
    }
    public void RemoveStun()
    {
        _stunSprite.SetActive(false);
    }

    public void SetCharge(float ratio)
    {
        if(ratio <= 0)
        {
            _shaker.ResetShake();
            _shaker.enabled = false;
            _emissionModule.enabled = false;
            return;
        }

        _shaker.enabled = true;
        _shaker.SetShake(ratio);

        if(ratio == 1) _emissionModule.enabled = true;
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
