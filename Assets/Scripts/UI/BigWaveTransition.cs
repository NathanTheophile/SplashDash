using UnityEngine;
using UnityEngine.UI;

public class BigWaveTransition : MonoBehaviour
{
    [SerializeField] private RectTransform _endPoint;
    [SerializeField] private RectTransform _startPoint;

    [SerializeField] private Image _image;
    [SerializeField] private GameObject _NextBG;

    private RectTransform _transform;

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        ResetWave();
    }

    public void ResetWave()
    {
        _transform.anchoredPosition = _startPoint.anchoredPosition;
    }
}
