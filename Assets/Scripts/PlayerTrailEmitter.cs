#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using UnityEngine;

public class PlayerTrailEmitter : MonoBehaviour
{
    [SerializeField] private TrailManager _TrailManager;
    [SerializeField] private float trailSpacing = 0.05f;
    [SerializeField] private Fish _Fish;

    private Vector2 _LastPosition;
    private DashTrail _ActiveDash;
    private bool _IsDashing;

    public DashTrail ActiveDash => _ActiveDash;

    private void Start()
    {
        _LastPosition = transform.position;
    }

    public void UpdateTrail(Vector2 position)
    {
        if (_IsDashing)
        {
            if (_ActiveDash != null)
                _ActiveDash.SetEnd(position);
            return;
        }

        if (Vector2.Distance(_LastPosition, position) < trailSpacing)
            return;

        if (_TrailManager != null && !_Fish.OnWater)
            _TrailManager.CreateWaterTrail(_LastPosition, position);
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
