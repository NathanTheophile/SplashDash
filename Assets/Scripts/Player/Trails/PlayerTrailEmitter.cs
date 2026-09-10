#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;

public class PlayerTrailEmitter : MonoBehaviour
{
    #region _________________________/ REFERENCES
    private TrailManager _TrailManager;
    [SerializeField] private Fish _Fish;
    [SerializeField] private PlayerPhysics2D _PlayerPhysicsSystem;

    #endregion

    #region _________________________/ RUNTIME VALUES

    private Vector2 _LastPosition;
    private float _DistanceSinceTrail;
    private DashTrail _ActiveDash;
    private bool _IsDashing;
    private bool _WasWaterTrailBlocked;
    private readonly WaterTrail[] _RecentTrails = new WaterTrail[3];
    private int _NextRecentTrailIndex;

    #endregion

    public DashTrail ActiveDash => _ActiveDash;
    public void SetTrailManager(TrailManager manager) => _TrailManager = manager;

    private void Awake()
    {
        if (_Fish == null)
            _Fish = GetComponent<Fish>();

        if (_PlayerPhysicsSystem == null)
            _PlayerPhysicsSystem = GetComponent<PlayerPhysics2D>();

        if (_TrailManager == null)
            _TrailManager = FindFirstObjectByType<TrailManager>();
    }

    public void RememberTrail(WaterTrail trail)
    {
        _RecentTrails[_NextRecentTrailIndex] = trail;
        _NextRecentTrailIndex = (_NextRecentTrailIndex + 1) % _RecentTrails.Length;
    }

    public bool IsRecentTrail(WaterTrail trail)
    {
        if (trail == null) return false;
        for (int i = 0; i < _RecentTrails.Length; i++)
            if (_RecentTrails[i] == trail) return true;
        return false;
    }

    private void LateUpdate() => UpdateTrail(transform.position);
    private void OnDisable() => EndDash();

    private void OnEnable()
    {
        _LastPosition = transform.position;
        _DistanceSinceTrail = 0f;
        _WasWaterTrailBlocked = IsWaterTrailBlocked;
    }

    private bool IsWaterTrailBlocked => _Fish.IsStunned || _PlayerPhysicsSystem.IsOnWater;

    public void UpdateTrail(Vector2 position)
    {
        if (_IsDashing)
        {
            if (_ActiveDash != null)
                _ActiveDash.SetEnd(position);
        }

        bool waterTrailBlocked = IsWaterTrailBlocked;
        if (waterTrailBlocked || waterTrailBlocked != _WasWaterTrailBlocked)
        {
            _WasWaterTrailBlocked = waterTrailBlocked;
            _LastPosition = position;
            _DistanceSinceTrail = 0f;
            return;
        }

        Vector2 lOffset = position - _LastPosition;
        float lDistance = lOffset.magnitude;
        if (lDistance <= 0f)
            return;

        Vector2 lDirection = lOffset / lDistance;
        float lSpacing = _TrailManager.WaterTrailSpacing;

        while (_DistanceSinceTrail + lDistance >= lSpacing)
        {
            float lDistanceToTrail = lSpacing - _DistanceSinceTrail;
            _LastPosition += lDirection * lDistanceToTrail;
            _TrailManager.CreateWaterTrail(_LastPosition, this);

            lDistance -= lDistanceToTrail;
            _DistanceSinceTrail = 0f;
        }

        _DistanceSinceTrail += lDistance;
        _LastPosition = position;
    }

    public void BeginDash(Vector2 dashDirection)
    {
        _IsDashing = true;

        _ActiveDash = _TrailManager.CreateDashTrail(transform.position, dashDirection);
    }

    public void EndDash()
    {
        _IsDashing = false;
        if (_ActiveDash != null)
            _ActiveDash.SetEnd(transform.position);

        _ActiveDash = null;

        _LastPosition = transform.position;
    }
}
