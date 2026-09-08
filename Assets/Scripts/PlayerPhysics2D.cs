#region _____________________________/ INFOS
//  AUTHOR : Splash&Dash (2026)
//  Engine : Unity
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics2D : MonoBehaviour
{
    #region _________________________/ REFERENCES
    private readonly Dictionary<Collider2D, WaterTrail> activeWaterOverlaps = new();
    private Fish _Fish;
    private PlayerTrailEmitter _TrailEmitter;

    #endregion

    #region _________________________/ STATE VALUES

    public bool IsOnWater
    {
        get
        {
            foreach (var overlap in activeWaterOverlaps)
            {
                if (overlap.Key == null || !overlap.Key.isActiveAndEnabled) continue;
                if (overlap.Value != null && _TrailEmitter != null && _TrailEmitter.IsRecentTrail(overlap.Value))
                    continue;
                return true;
            }
            return false;
        }
    }

    #endregion

    private void OnDisable() => ClearContacts();

    private void Awake()
    {
        _Fish = GetComponent<Fish>();
        _TrailEmitter = GetComponent<PlayerTrailEmitter>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        WaterTrail waterTrail = other.GetComponent<WaterTrail>();
        if (other.CompareTag("Water") || waterTrail != null)
            activeWaterOverlaps[other] = waterTrail;
        else if (_Fish != null && _Fish.IsDashing)
        {
            DashTrail trail = other.GetComponentInParent<DashTrail>();
            if (trail != null && (_TrailEmitter == null || trail != _TrailEmitter.ActiveDash))
                Destroy(trail.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) => activeWaterOverlaps.Remove(other);

    public void ClearContacts() => activeWaterOverlaps.Clear();
}
