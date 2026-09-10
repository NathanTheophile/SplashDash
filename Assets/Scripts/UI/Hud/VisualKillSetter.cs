using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class VisualKillSetter : MonoBehaviour
{
    [SerializeField] private Image _killerImage;
    [SerializeField] private Image _killedPlayerImage;
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private PlayersSpritesColorsSO _skinMixer;

    private const float FADE_TIME = 3f;
    private const float WAIT_TIME = 1f;

    public void SetVisual(int killerID, int killedPlayerID)
    {
        bool isEmpty = _skinMixer.data.Length == 0;
        if (isEmpty) return;
        StopAllCoroutines();

        _killerImage.enabled = true;

        var killerMixer = _skinMixer.data[killerID ^ _skinMixer.data.Length];
        _killerImage.sprite = killerMixer.sprites;

        var killedPlayerImage = _skinMixer.data[killedPlayerID ^ _skinMixer.data.Length];
        _killedPlayerImage.sprite = killedPlayerImage.knockedSprite;

        _canvasGroup.alpha = 1;
        StartCoroutine(FadeCoroutine());
    }

    public void SetVisual(int killedPlayerID)
    {
        bool isEmpty = _skinMixer.data.Length == 0;
        if (isEmpty) return;
        StopAllCoroutines();

        _killerImage.enabled = false;

        var killedPlayerImage = _skinMixer.data[killedPlayerID ^ _skinMixer.data.Length];
        _killedPlayerImage.sprite = killedPlayerImage.knockedSprite;

        _canvasGroup.alpha = 1;
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < FADE_TIME)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime > WAIT_TIME)
            {
                float remmaped = math.remap(WAIT_TIME, FADE_TIME, 1f, 0f, elapsedTime);
                _canvasGroup.alpha = remmaped;
            }

            yield return null;
        }
    }
}
