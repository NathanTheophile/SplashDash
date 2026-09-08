#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyCallback, MyStruct
#endregion

using System.Collections.Generic;
using UnityEngine;

public class TrailManager : MonoBehaviour
{
    #region _________________________/ REFERENCES
    [SerializeField] private WaterTrail _WaterTrailPrefab;
    [SerializeField] private DashTrail _DashTrailPrefab;

    #endregion

    #region _________________________/ TUNING VALUES
    [SerializeField, Min(0.05f)] private float _WaterTrailSpacing = 0.2f;

    #endregion

    #region _________________________/ RUNTIME VALUES

    private readonly List<GameObject> _Trails = new();

    #endregion

    public void CreateWaterTrail(Vector2 pStart, Vector2 pEnd, PlayerTrailEmitter owner)
    {
        Vector2 lOffset = pEnd - pStart;
        float lDistance = lOffset.magnitude;
        int lCount = Mathf.Max(1, Mathf.CeilToInt(lDistance / _WaterTrailSpacing));
        Vector2 lDirection = lDistance > Mathf.Epsilon ? lOffset / lDistance : Vector2.zero;

        for (int i = 0; i < lCount; i++)
        {
            Vector2 lPosition = pStart + lDirection * (i * _WaterTrailSpacing);
            WaterTrail lTrail = Instantiate(_WaterTrailPrefab, lPosition, Quaternion.identity, transform);
            _Trails.Add(lTrail.gameObject);
            if (owner != null) owner.RememberTrail(lTrail);
        }
    }

    public DashTrail CreateDashTrail(Vector2 pPosition, Vector2 pDirection)
    {
        Debug.Log("Creating dash trail at " + pPosition + " with direction " + pDirection);
        DashTrail lTrail = Instantiate(_DashTrailPrefab);
        lTrail.Begin(pPosition, pDirection);

        _Trails.Add(lTrail.gameObject);

        return lTrail;
    }

    public void ClearTrails()
    {
        foreach (GameObject trail in _Trails)
        {
            if (trail != null)
                Destroy(trail);
        }

        _Trails.Clear();
    }
}
