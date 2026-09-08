#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;

public class PlayerTrailEmitter : MonoBehaviour
{
    #region _________________________/ REFERENCES
    [SerializeField] private TrailManager _TrailManager;
    [SerializeField] private Fish _Fish;
    [SerializeField] private PlayerPhysics2D physicsContacts;

    #endregion

    #region _________________________/ TUNING VALUES
    [SerializeField] private float trailSpacing = 0.05f;

    #endregion

    #region _________________________/ RUNTIME VALUES

    private Vector2 _LastPosition;
    private DashTrail _ActiveDash;
    private bool _IsDashing;
    private bool wasSillonBlocked;
    private readonly WaterTrail[] recentTrails = new WaterTrail[3];
    private int nextRecentTrail;

    #endregion

    public DashTrail ActiveDash => _ActiveDash;
    public void SetTrailManager(TrailManager manager) => _TrailManager = manager;

    private void Awake()
    {
        if (_Fish != null && physicsContacts != null)
            return;

        enabled = false;
    }

    public void RememberTrail(WaterTrail trail)
    {
        recentTrails[nextRecentTrail] = trail;
        nextRecentTrail = (nextRecentTrail + 1) % recentTrails.Length;
    }

    public bool IsRecentTrail(WaterTrail trail)
    {
        if (trail == null) return false;
        for (int i = 0; i < recentTrails.Length; i++)
            if (recentTrails[i] == trail) return true;
        return false;
    }

    private void LateUpdate() => UpdateTrail(transform.position);
    private void OnDisable() => EndDash();

    private void OnEnable()
    {
        _LastPosition = transform.position;
        wasSillonBlocked = IsSillonBlocked;
    }

    private bool IsSillonBlocked => _Fish.IsStunned || physicsContacts.IsOnWater;

    public void UpdateTrail(Vector2 position)
    {
        if (_IsDashing)
        {
            if (_ActiveDash != null)
                _ActiveDash.SetEnd(position);
        }

        bool sillonBlocked = IsSillonBlocked;
        if (sillonBlocked || sillonBlocked != wasSillonBlocked)
        {
            wasSillonBlocked = sillonBlocked;
            _LastPosition = position;
            return;
        }

        if (Vector2.Distance(_LastPosition, position) < trailSpacing)
            return;

        if (_TrailManager != null)
            _TrailManager.CreateWaterTrail(_LastPosition, position, this);
        _LastPosition = position;
    }

    public void BeginDash(Vector2 direction)
    {
        _IsDashing = true;

        _ActiveDash = _TrailManager != null
            ? _TrailManager.CreateDashTrail(transform.position, direction)
            : null;
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
