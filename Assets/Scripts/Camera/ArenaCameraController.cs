using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera), typeof(CinemachineImpulseListener))]
public class ArenaCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera _CinemachineCamera;
    [SerializeField] private CinemachineImpulseSource _ImpulseSource;
    [SerializeField] private BoxCollider2D _CameraBounds;

    [Header("Zoom")]
    [SerializeField, Min(0.1f)] private float _MinimumOrthographicSize = 5f;
    [SerializeField, Min(0.1f)] private float _MaximumOrthographicSize = 8f;
    [SerializeField, Min(0f)] private float _GroupPadding = 1.5f;
    [SerializeField, Min(0.01f)] private float _ZoomSmoothTime = 0.75f;

    [Header("Movement")]
    [SerializeField, Min(0.01f)] private float _PositionSmoothTime = 0.35f;

    [Header("Death Shake")]
    [SerializeField, Min(0f)] private float _DeathShakeForce = 0.4f;

    private readonly List<Fish> _Fishes = new();
    private Vector3 _PositionVelocity;
    private float _ZoomVelocity;

    private void Awake()
    {
        if (_CinemachineCamera == null)
            _CinemachineCamera = GetComponent<CinemachineCamera>();

        _CinemachineCamera.Lens.ModeOverride = LensSettings.OverrideModes.Orthographic;
        _CinemachineCamera.Lens.OrthographicSize = _MaximumOrthographicSize;
    }

    private void OnEnable() => Fish.OnPlayerDeath += OnPlayerDeath;

    private void Start() => RefreshFishes();

    private void LateUpdate()
    {
        if (_Fishes.Count == 0)
            RefreshFishes();

        RemoveInvalidFishes();

        if (_Fishes.Count == 0)
            return;

        Bounds lFishesBounds = GetFishesBounds();
        float lTargetZoom = GetTargetZoom(lFishesBounds);

        _CinemachineCamera.Lens.OrthographicSize = Mathf.SmoothDamp(
            _CinemachineCamera.Lens.OrthographicSize,
            lTargetZoom,
            ref _ZoomVelocity,
            _ZoomSmoothTime);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            GetTargetPosition(lFishesBounds),
            ref _PositionVelocity,
            _PositionSmoothTime);
    }

    private void OnDisable() => Fish.OnPlayerDeath -= OnPlayerDeath;

    private void RefreshFishes()
    {
        _Fishes.Clear();
        _Fishes.AddRange(FindObjectsByType<Fish>(FindObjectsInactive.Include, FindObjectsSortMode.None));
    }

    private void RemoveInvalidFishes()
    {
        for (int i = _Fishes.Count - 1; i >= 0; i--)
        {
            if (_Fishes[i] == null || !_Fishes[i].gameObject.activeInHierarchy)
                _Fishes.RemoveAt(i);
        }
    }

    private void OnPlayerDeath(int pPlayerIndex, int pKillerIndex)
    {
        for (int i = _Fishes.Count - 1; i >= 0; i--)
        {
            if (_Fishes[i] == null || _Fishes[i].FishIndex != pPlayerIndex)
                continue;

            Vector3 lDeathPosition = _Fishes[i].transform.position;
            _Fishes.RemoveAt(i);

            if (_ImpulseSource != null && _DeathShakeForce > 0f)
                _ImpulseSource.GenerateImpulseAtPositionWithVelocity(lDeathPosition, new Vector3(1f, 1f, 0f) * _DeathShakeForce);

            return;
        }
    }

    private Bounds GetFishesBounds()
    {
        Bounds lBounds = new(_Fishes[0].transform.position, Vector3.zero);

        for (int i = 1; i < _Fishes.Count; i++)
            lBounds.Encapsulate(_Fishes[i].transform.position);

        return lBounds;
    }

    private float GetTargetZoom(Bounds pFishesBounds)
    {
        float lAspectRatio = GetAspectRatio();
        float lVerticalSize = pFishesBounds.extents.y + _GroupPadding;
        float lHorizontalSize = (pFishesBounds.extents.x + _GroupPadding) / lAspectRatio;
        float lRequiredSize = Mathf.Max(lVerticalSize, lHorizontalSize);

        return Mathf.Clamp(lRequiredSize, _MinimumOrthographicSize, _MaximumOrthographicSize);
    }

    private Vector3 GetTargetPosition(Bounds pFishesBounds)
    {
        if (_CameraBounds == null)
            return new Vector3(pFishesBounds.center.x, pFishesBounds.center.y, transform.position.z);

        float lHalfHeight = _CinemachineCamera.Lens.OrthographicSize;
        float lHalfWidth = lHalfHeight * GetAspectRatio();
        Bounds lBounds = _CameraBounds.bounds;

        float lX = Mathf.Clamp(pFishesBounds.center.x, lBounds.min.x + lHalfWidth, lBounds.max.x - lHalfWidth);
        float lY = Mathf.Clamp(pFishesBounds.center.y, lBounds.min.y + lHalfHeight, lBounds.max.y - lHalfHeight);

        return new Vector3(lX, lY, transform.position.z);
    }

    private float GetAspectRatio()
    {
        Camera lMainCamera = Camera.main;
        return lMainCamera != null ? lMainCamera.aspect : 16f / 9f;
    }
}
