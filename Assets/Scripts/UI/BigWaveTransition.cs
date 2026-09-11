using System.Collections;
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

    [SerializeField] private GameObject _bigWave;

    [SerializeField] private float _waveTime = 1f;
    [SerializeField] private float _wavefadeTime = 0.5f;
    [SerializeField] private float _finalTime = 0.7f;
    [SerializeField] private float _frequency = 2f;
    [SerializeField] private float _heigth = 1f;
    private RectTransform _transform;
    private bool _isReloadingGameplay;

    public UnityEvent OnAnimationHalfed;
    public UnityEvent OnCanvasFade;

    public void PrepareGameplay()
    {
        Scene scene = SceneManager.GetSceneByName("Gameplay");
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    public void PrepareReplay()
    {
        if (_isReloadingGameplay) return;

        _isReloadingGameplay = true;
        StartCoroutine(ReloadGameplayCoroutine());
    }

    private IEnumerator ReloadGameplayCoroutine()
    {
        Scene gameplayScene = SceneManager.GetSceneByName("Gameplay");
        if (gameplayScene.isLoaded)
            yield return SceneManager.UnloadSceneAsync(gameplayScene);

        SceneManager.sceneLoaded += OnReplaySceneLoaded;
        yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    private void OnReplaySceneLoaded(Scene pScene, LoadSceneMode pMode)
    {
        if (pScene.name != "Gameplay") return;

        SceneManager.sceneLoaded -= OnReplaySceneLoaded;
        _isReloadingGameplay = false;
        PlayerManager.Instance.SetupGameplay(pScene);
        GameManager.Instance.SetupGameplay(pScene);
        FindFirstObjectByType<LevelManager>().SpawnLevel(pScene);
        InputManager.Instance.RespawnPlayers();
        TransitionManager.Instance.GoToGameplay();
    }

    private void OnSceneLoaded(Scene pScene, LoadSceneMode pMode)
    {
        if (pScene.name != "Gameplay") return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerManager.Instance.SetupGameplay(pScene);
        GameManager.Instance.SetupGameplay(pScene);
        FindFirstObjectByType<LevelManager>().SpawnLevel(pScene);
        InputManager.Instance.EnableDeviceConnection(true);
    }

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
    }



    public void DoTransition()
    {
        gameObject.SetActive(true);
        _bigWave.SetActive(true);
        StartCoroutine(StartTransitionCoroutine());
    }

    public void EndTransition()
    {
        _bigWave.SetActive(true);
        _NextBG.gameObject.SetActive(true);
        gameObject.SetActive(true);
        StartCoroutine(FinishTransitionCoroutine());
    }

    private void ResetWave()
    {
        _transform.anchoredPosition = _startPointWave.anchoredPosition;
        _image.color = new Color(1, 1, 1, 1);
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
                _NextBG.anchoredPosition = _startPointBG.anchoredPosition;
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

}
