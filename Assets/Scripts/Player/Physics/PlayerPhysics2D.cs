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
    private readonly Dictionary<Collider2D, WaterTrail> _WaterContactsByCollider = new();
    private readonly Dictionary<Collider2D, Vector2> _CurrentDirectionsByCollider = new();
    private Collider2D _SelectedCurrentCollider;
    private Fish _Fish;
    private PlayerTrailEmitter _TrailEmitter;

    #endregion

    #region _________________________/ STATE VALUES

    public bool IsOnWater => HasWaterOverlap();
    public Vector2 CurrentDirection => GetCurrentDirection();

    #endregion

    private void OnDisable() => ClearContacts();

    private void Awake()
    {
        _Fish = GetComponent<Fish>();
        _TrailEmitter = GetComponent<PlayerTrailEmitter>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DashTrail trail = other.GetComponentInParent<DashTrail>();
        if (trail != null)
        {
            HandleCurrentContact(other, trail);
            return;
        }

        WaterTrail waterTrail = other.GetComponent<WaterTrail>();
        if (other.CompareTag("Water") || waterTrail != null)
            _WaterContactsByCollider[other] = waterTrail;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_Fish.IsDashing) return;

        DashTrail trail = other.GetComponentInParent<DashTrail>();
        if (trail != null)
            HandleCurrentContact(other, trail);
    }

    private void HandleCurrentContact(Collider2D pCollider, DashTrail pTrail)
    {
        if (_Fish.IsDashing && pTrail != _TrailEmitter.ActiveDash)
        {
            pTrail.RemoveSquare(pCollider);
            _CurrentDirectionsByCollider.Remove(pCollider);
            return;
        }

        _CurrentDirectionsByCollider[pCollider] = pTrail.Direction;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _CurrentDirectionsByCollider.Remove(other);
        _WaterContactsByCollider.Remove(other);
    }
    
    

    public void ClearContacts()
    {
        _WaterContactsByCollider.Clear();
        _CurrentDirectionsByCollider.Clear();
        _SelectedCurrentCollider = null;
    }

    private bool HasWaterOverlap()
    {
        foreach (var waterContact in _WaterContactsByCollider)
        {
            if (waterContact.Key == null) continue;
            if (waterContact.Value != null && _TrailEmitter.IsRecentTrail(waterContact.Value)) continue;
            return true;
        }

        return false;
    }

    private Vector2 GetCurrentDirection()
    {
        if (_SelectedCurrentCollider != null && _SelectedCurrentCollider.isActiveAndEnabled &&
            _CurrentDirectionsByCollider.TryGetValue(_SelectedCurrentCollider, out Vector2 currentDirection))
            return currentDirection;

        _SelectedCurrentCollider = null;
        foreach (var currentContact in _CurrentDirectionsByCollider)
        {
            Collider2D currentCollider = currentContact.Key;
            if (currentCollider == null || !currentCollider.isActiveAndEnabled) continue;
            _SelectedCurrentCollider = currentCollider;
            return currentContact.Value;
        }

        return Vector2.zero;
    }
}
