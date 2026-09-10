using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScaleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Target")]
    [SerializeField] private RectTransform targetTransform;

    [Header("Scale Values")]
    [SerializeField] private Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] private Vector3 pressedScale = new Vector3(0.9f, 0.9f, 0.9f);

    [Header("Animation")]
    [SerializeField] private float transitionDuration = 0.1f;

    private Vector3 defaultScale;
    private Coroutine scaleCoroutine;
    private bool isHovered;

    private void Awake()
    {
        if (targetTransform == null)
            targetTransform = GetComponent<RectTransform>();

        defaultScale = targetTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        AnimateScale(hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        AnimateScale(defaultScale);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AnimateScale(pressedScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AnimateScale(isHovered ? hoverScale : defaultScale);
    }

    private void AnimateScale(Vector3 targetScale)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(ScaleRoutine(targetScale));
    }

    private IEnumerator ScaleRoutine(Vector3 targetScale)
    {
        Vector3 startScale = targetTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / transitionDuration;
            targetTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        targetTransform.localScale = targetScale;
    }

    private void OnDisable()
    {
        if (targetTransform != null)
            targetTransform.localScale = defaultScale;
    }
}