using System.Collections;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class BigWaveTransition : MonoBehaviour
{
    [SerializeField] private RectTransform _endPointBG;
    [SerializeField] private RectTransform _startPointBG;

    [SerializeField] private RectTransform _endPointWave;
    [SerializeField] private RectTransform _startPointWave;

    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _NextBG;

    [SerializeField] private AnimationCurve _waveCurve;
    [SerializeField] private AnimationCurve _endCurve;

    [SerializeField] private float _waveTime = 1f;
    [SerializeField] private float _wavefadeTime = 0.5f;
    [SerializeField] private float _finalTime = 0.7f;
    [SerializeField] private float _frequency = 2f;
    [SerializeField] private float _heigth = 1f;


    private RectTransform _transform;

    public UnityEvent OnAnimationHalfed;
    public UnityEvent OnCanvasFade;

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
    }



    public void DoTransition()
    {
        StartCoroutine(StartTransitionCoroutine());
    }

    public void EndTransition()
    {
        StartCoroutine(FinishTransitionCoroutine());
    }

    private void ResetWave()
    {
        _transform.anchoredPosition = _startPointWave.anchoredPosition;
    }

    private IEnumerator DoFade()
    {
        _NextBG.gameObject.SetActive(true);

        OnCanvasFade?.Invoke();

        float elapsedTime = 0f;

        while (elapsedTime < _wavefadeTime)
        {
            elapsedTime += Time.deltaTime;
            float ratio = elapsedTime / _wavefadeTime;

            _image.color = new Color(1f, 1f, 1f, 1 - ratio);

            yield return null;
        }
    }

    private IEnumerator StartTransitionCoroutine()
    {
        float elapsedTime = 0f;
        bool active = false;

        ResetWave();

        while (elapsedTime < _waveTime)
        {
            elapsedTime += Time.deltaTime;
            float ratio =  _waveCurve.Evaluate(elapsedTime / _waveTime);

            float xPos = Mathf.Lerp(_startPointWave.anchoredPosition.x, _endPointWave.anchoredPosition.x, ratio);
            float yPos = _startPointWave.anchoredPosition.y + (Mathf.Sin(ratio * _frequency) + 1) * 0.5f * _heigth;

            _transform.anchoredPosition = new Vector2(xPos, yPos);

            if(elapsedTime > _waveTime - _wavefadeTime && !active)
            {
                StartCoroutine(DoFade());
                active = true;
            }

            yield return null;
        }

        OnAnimationHalfed?.Invoke();

        _image.color = new Color(1, 1, 1, 0);
    }

    private IEnumerator FinishTransitionCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _finalTime)
        {
            elapsedTime += Time.deltaTime;
            float ratio = _endCurve.Evaluate(elapsedTime / _finalTime);

            float yPos = Mathf.Lerp(_startPointBG.anchoredPosition.y, _endPointBG.anchoredPosition.y, ratio);

            float xPos = _startPointBG.anchoredPosition.x + (Mathf.Cos(ratio * _frequency) * -1 + 1) * 0.5f * _heigth;

            _NextBG.anchoredPosition = new Vector2(xPos, yPos);
            yield return null;
        }
    }

    private async Task LoadScene(int id)
    {
        await SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive);
    }
}
